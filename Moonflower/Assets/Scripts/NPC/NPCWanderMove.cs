using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWanderMove : MonoBehaviour, IMovement, INPCMovement
{
    public Actions Action { get; set; }
    public bool Jumping { get; set; }

    public bool Active { get; set; }
    public bool AvoidsPlayer { get; set; } = false;
    public bool stunned;
    public bool swinging;

    public GameObject player;
    GameObject self;
    //bool canWander;

    NavMeshAgent agent;
    Vector3 destination;
    Vector3 wanderAreaOrigin;
    float wanderAreaRadius = 5f; //default

    float avoidsPlayerRadius = 10f;

    const float bufferDist = .2f; //max dist from destination point before going somewhere else
    int pauseCount = 0; //keeps track of how long NPC has been chilling at destination
    int lingerLength; //length of pause 
    const int smallPause = 1, largePause = 4; //max and min pause lengths


    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    //initialize so player CANNOT wander CANNOT engage
    public NPCWanderMove(GameObject selfOb, GameObject playerOb)
    {
        Active = false; 
        commonSetup(selfOb, playerOb);

        //default
        destination = new Vector3(0, 0, 0);
        wanderAreaOrigin = new Vector3(0, 0, 0);

    }

    //initialize so player CAN wander CANNOT engage
    public NPCWanderMove(GameObject selfOb, GameObject playerOb, Vector3 wanderOrigin, float wanderRadius)
    {
        Active = true; 
        commonSetup(selfOb, playerOb);

        wanderAreaOrigin = wanderOrigin;
        wanderAreaRadius = wanderRadius;
        destination = getRandomDest();
        self.transform.position = destination;
    }

    //called by all constructors
    private void commonSetup(GameObject selfOb, GameObject playerOb)
    {
        player = playerOb;
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();

        lingerLength = getRandomPause();
    }


    public void UpdateMovement()
    {
        if (Active)
        {
            float destDistFromPlayer = Vector3.Distance(player.transform.position, destination);
            if (AvoidsPlayer && destDistFromPlayer < avoidsPlayerRadius)
            {
                agent.isStopped = false;
                //Action = Actions.Walking;
                destination = getRandomDest();
            }

            bool atDest = getXZDist(self.transform.position, destination) <= bufferDist;
            if (atDest && !agent.isStopped)
            {
                Chill();
                Action = Actions.Chilling;
                agent.isStopped = true;
            }
            else if (atDest)
            {
                if (pauseCount == lingerLength)
                {
                    destination = getRandomDest();
                    GoHere(destination);

                    lingerLength = getRandomPause();
                    pauseCount = 0;
                }
                else
                {
                    pauseCount++;
                }
            }
            else
            {
                GoHere(destination);
            }
        }
    }

    //make NPC start wandering again
    public void ResumeMovement()
    {
        Active = true;
        destination = getRandomDest();
        GoHere(destination);

        lingerLength = getRandomPause();
        pauseCount = 0;
    }

    public void SetOrigin(Vector3 loc)
    {
        wanderAreaOrigin = loc;
    }

    public void SetRadius(float rad)
    {
        wanderAreaRadius = rad; 
    }

    public void SetArea(Vector3 loc, float rad)
    {
        wanderAreaOrigin = loc;
        wanderAreaRadius = rad;
    }

    //make sure npc give player a bit of space 
    public void SetAvoidsPlayerRadius(float dist)
    {
        AvoidsPlayer = true;
        avoidsPlayerRadius = dist;
    }

    //stop NPC movement
    public void Chill()
    {
        Action = Actions.Chilling;
        agent.isStopped = true;
    }

    //send NPC to location
    public void GoHere(Vector3 loc)
    {
        if (Action != Actions.Running)
            Action = Actions.Walking;
        agent.isStopped = false;
        agent.SetDestination(loc);
    }

    // always gets a reachable point on the navmesh
    private Vector3 getRandomDest()
    {
        return getRandomDest(wanderAreaOrigin, wanderAreaRadius);
    }

    // always gets a reachable point on the navmesh around origin 
    private Vector3 getRandomDest(Vector3 origin, float radius)
    {
        int count = 0;
        int bailCount = 20;

        float x, z;
        NavMeshHit hit;
        bool viablePosition = true, viablePath = true, notTooClose = true;
        do
        {
            x = origin.x + Random.Range(-radius, radius);
            z = origin.z + Random.Range(-radius, radius);
            viablePosition = NavMesh.SamplePosition(new Vector3(x, self.transform.position.y, z), out hit, radius, NavMesh.AllAreas);
            viablePath = agent.CalculatePath(hit.position, new NavMeshPath());

            if (AvoidsPlayer && viablePath)
            {
                notTooClose = getXZDist(hit.position, player.transform.position) > avoidsPlayerRadius;
            }
            count++;

        } while ((!viablePosition || !viablePath || !notTooClose) && count < bailCount);

        if (count == bailCount)
        {
            return new Vector3(x, 0, z);
        }
        else
        {
            return hit.position;
        }
    }

    private int getRandomPause()
    {
        return Random.Range(smallPause * 60, largePause * 60);
    }

    private float getXZDist(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z);

    }

    // Disable player combat controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }

}
