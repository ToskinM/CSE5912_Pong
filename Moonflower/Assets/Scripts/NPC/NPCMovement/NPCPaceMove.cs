using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPaceMove : MonoBehaviour, IMovement, INPCMovement
{
    public Actions Action { get; set; }
    public bool Jumping { get; set; }

    public bool Active { get; set; }
    public bool AvoidsTarget { get; set; } = false;
    public bool stunned;
    public bool swinging;

    public GameObject target;
    GameObject self;
    //bool canWander;

    NavMeshAgent agent;
    Vector3 destination;

    Vector3 origin;
    float paceDist;
    float overshoot = 0.5f;
    bool atOrigin = true; 

    float bufferDist = 0.1f; 
    public float baseSpeed; 

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    //initialize so player CANNOT wander CANNOT engage
    public NPCPaceMove(GameObject selfOb)
    {
        Active = true; 
        commonSetup(selfOb);

        //default
        destination = new Vector3(0, 0, 0);
        origin = new Vector3(0, 0, 0);
        paceDist = 0; 

    }

    //initialize so player CAN wander CANNOT engage
    public NPCPaceMove(GameObject selfOb, Vector3 originSpot, float distance)
    {
        Active = true; 
        commonSetup(selfOb);

        origin = originSpot;
        paceDist = distance;

        destination = getRandomDest();
        self.transform.position = destination;
    }

    //called by all constructors
    private void commonSetup(GameObject selfOb)
    {
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();

        baseSpeed = agent.speed;
    }


    public void UpdateMovement()
    {
        if (Active)
        {
            agent.speed = baseSpeed; 

            bool atDest = getXZDist(self.transform.position, destination) <= bufferDist;
            if (atDest)
            {
                destination = getRandomDest();
            }
            GoHere(destination);
        }
    }

    //make NPC start wandering again
    public void ResumeMovement()
    {
        Active = true;
        destination = getRandomDest();
        GoHere(destination);
    }

    public void SetOrigin(Vector3 loc)
    {
        origin = loc;
    }

    public void SetDistance(float rad)
    {
        paceDist = rad; 
    }

    public void SetPath(Vector3 loc, float rad)
    {
        origin = loc;
        paceDist = rad;
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

    // always gets a reachable point on the navmesh around origin 
    private Vector3 getRandomDest()
    {
        int count = 0;
        int bailCount = 20;

        float x, z;
        NavMeshHit hit;
        bool viablePosition = true, viablePath = true;
        do
        {
            if (atOrigin)
            {
                x = origin.x + paceDist + Random.Range(-overshoot, overshoot);
                z = origin.z + Random.Range(-overshoot, overshoot);
                atOrigin = false; 
            }
            else
            {
                x = origin.x + Random.Range(-overshoot, overshoot);
                z = origin.z + Random.Range(-overshoot, overshoot);
                atOrigin = true; 
            }
            viablePosition = NavMesh.SamplePosition(new Vector3(x, self.transform.position.y, z), out hit, paceDist, NavMesh.AllAreas);
            viablePath = agent.CalculatePath(hit.position, new NavMeshPath());

            count++;

        } while ((!viablePosition || !viablePath) && count < bailCount);

        if (count == bailCount)
        {
            return new Vector3(x, 0, z);
        }
        else
        {
            return hit.position;
        }
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
