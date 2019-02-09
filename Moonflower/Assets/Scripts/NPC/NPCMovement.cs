using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour, IMovement
{
    public Actions Action { get; set; }
    public bool Jumping { get; set; }

    public bool Wandering { get { return wandering; } } //currently wandering
    public bool CanWander { get { return canWander; } } //capable of wandering
    public bool CanEngage { get; set; } //capable of being talked to 
    public bool Engaging { get { return engaging; } } //currently being talked to 
    public bool AvoidsPlayer { get; set; } = false;

    GameObject player;
    GameObject self;
    bool canWander;
    bool wandering;
    bool engaging;

    NavMeshAgent agent;
    Vector3 dest;
    Vector3 wanderAreaOrigin;
    float wanderAreaRadius = 5f;

    float avoidsPlayerRadius = 10f;

    const float bufferDist = .2f; //max dist from destination point before going somewhere else
    int pauseCount = 0; //keeps track of how long NPC has been chilling at destination
    int lingerLength; //length of pause 
    const int smallPause = 1, largePause = 4; //max and min pause lengths

    float engagementRadius = 0f;
    float bufferRadius = 3f;
    float tooCloseRadius = 2.5f;

    //initialize so player CANNOT wander CANNOT engage
    public NPCMovement(GameObject selfOb, GameObject playerOb)
    {
        player = playerOb;
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();

        canWander = false;
        wandering = false;

        CanEngage = false;
        engaging = false;

        //default
        dest = new Vector3(0, 0, 0);
        wanderAreaOrigin = new Vector3(0, 0, 0);

        lingerLength = getRandomPause();
    }

    //initialize so player CAN wander CANNOT engage
    public NPCMovement(GameObject selfOb, GameObject playerOb, Vector3 wanderOrigin, float wanderRadius)
    {
        player = playerOb;
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();

        canWander = true;
        wandering = true;

        CanEngage = false;
        engaging = false;

        wanderAreaOrigin = wanderOrigin;
        wanderAreaRadius = wanderRadius;
        dest = getRandomDest();
        self.transform.position = dest;

        lingerLength = getRandomPause();

    }

    //initialize so player CAN wander CAN engage
    public NPCMovement(GameObject selfOb, GameObject playerOb, Vector3 wanderOrigin, float wanderRadius, float maxEngagementDistance)
    {
        player = playerOb;
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();


        canWander = true;
        wandering = true;

        CanEngage = true;
        engaging = false;

        engagementRadius = maxEngagementDistance;

        wanderAreaOrigin = wanderOrigin;
        wanderAreaRadius = wanderRadius;
        dest = getRandomDest();
        self.transform.position = dest;

        lingerLength = getRandomPause();

    }

    //initialize so player CANNOT wander CAN engage
    public NPCMovement(GameObject selfOb, GameObject playerOb, float maxEngagementDistance)
    {
        player = playerOb;
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();

        canWander = false;
        wandering = false;

        CanEngage = true;
        engaging = false;

        engagementRadius = maxEngagementDistance;

        //default
        dest = new Vector3(0, 0, 0);
        wanderAreaOrigin = new Vector3(0, 0, 0);

        lingerLength = getRandomPause();
    }


    public void UpdateMovement()
    {
        if (Wandering)
        {
            wander();
        }

        if (CanEngage)
        {
            if (!Engaging)
            {
                float distFromPlayer = Vector3.Distance(player.transform.position, self.transform.position);
                SetEngaging(distFromPlayer <= engagementRadius);
            }
            if (Engaging)
            {
                engage();
            }
        }


    }

    private void engage()
    {
        float distFromPlayer = Vector3.Distance(player.transform.position, self.transform.position);
        SetWandering(false);
        if (distFromPlayer < bufferRadius)
        {
            if (distFromPlayer < tooCloseRadius)
            {
                Vector3 targetDirection = self.transform.position - player.transform.position;
                Vector3 newDest = self.transform.position + targetDirection.normalized * 10;
                dest = getRandomDest(newDest, 1f);
                GoHere(dest); 
            }
            else
            {
                Chill();
            }
        }
        else
            GoHere(player.transform.position);


    }

    private void wander()
    {
        //Debug.Log("I am wandering!");
        float destDistFromPlayer = Vector3.Distance(player.transform.position, dest);
        if (AvoidsPlayer && destDistFromPlayer < avoidsPlayerRadius)
        {
            agent.isStopped = false;
            Action = Actions.Walking;
            dest = getRandomDest();
        }

        bool atDest = getXZDist(self.transform.position, dest) <= bufferDist;
        if (atDest && !agent.isStopped)
        {
            Action = Actions.Chilling;
            agent.isStopped = true;
        }
        else if (atDest)
        {
            if (pauseCount == lingerLength)
            {
                dest = getRandomDest();
                GoHere(dest);

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
            GoHere(dest); 
        }
    }

    public void SetWandering(bool isWandering)
    {
        if (canWander)
        {
            wandering = isWandering;
        }
    }

    public void SetEngaging(bool isEngaging)
    {
        if (CanEngage)
        {
            engaging = isEngaging;
        }
    }

    //make sure npc give player a bit of space 
    public void SetAvoidsPlayerRadius(float dist)
    {
        AvoidsPlayer = true;
        avoidsPlayerRadius = dist;
    }

    public void StayClose(Vector3 loc)
    {
        if(getXZDist(dest,loc) > wanderAreaRadius)
        {
            wanderAreaOrigin = loc;
            dest = getRandomDest(); 
        }
    }

    //set distances that NPC like when engaging with the player 
    public void SetEngagementDistances(float engagementDist, float bufferDist, float tooCloseDist)
    {
        engagementRadius = engagementDist;
        bufferRadius = bufferDist;
        tooCloseRadius = tooCloseDist;
    }

    //where can NPC wander (enables wandering) 
    public void SetWanderArea(Vector3 origin, float radius)
    {
        canWander = true;
        wanderAreaOrigin = origin;
        wanderAreaRadius = radius;
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
        Action = Actions.Walking;
        agent.isStopped = false;
        agent.SetDestination(loc);
    }

    //make NPC start wandering again
    public void ResumeWandering()
    {
        if (canWander)
        {
            SetWandering(true);
            dest = getRandomDest();
            GoHere(dest); 

            lingerLength = getRandomPause();
            pauseCount = 0;
        }
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

            if(AvoidsPlayer && viablePath)
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



}
