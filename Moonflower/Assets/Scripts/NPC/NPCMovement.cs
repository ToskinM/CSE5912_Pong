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
    public bool FollowingPlayer { get; set; } = false; 

    GameObject player;
    GameObject self;
    bool canWander;
    bool wandering;
    bool engaging;
    private bool stickingAround = false;
    int stickAroundCount = 0;
    int stickAroundMax = 10; 

    NavMeshAgent agent;
    Vector3 dest;
    Vector3 wanderAreaOrigin;
    float wanderAreaRadius = 5f; //default
    float followDist = 5; //default
    float baseSpeed;
    float summonDist = 0; 

    float avoidsPlayerRadius = 10f;

    const float bufferDist = .2f; //max dist from destination point before going somewhere else
    int pauseCount = 0; //keeps track of how long NPC has been chilling at destination
    int lingerLength; //length of pause 
    const int smallPause = 1, largePause = 4; //max and min pause lengths

    //default
    float engagementRadius = 0f;
    float bufferRadius = 3f;
    float tooCloseRadius = 2.5f;

    //initialize so player CANNOT wander CANNOT engage
    public NPCMovement(GameObject selfOb, GameObject playerOb)
    {
        commonSetup(selfOb, playerOb);

        canWander = false;
        wandering = false;

        CanEngage = false;
        engaging = false;

        //default
        dest = new Vector3(0, 0, 0);
        wanderAreaOrigin = new Vector3(0, 0, 0);

    }

    //initialize so player CAN wander CANNOT engage
    public NPCMovement(GameObject selfOb, GameObject playerOb, Vector3 wanderOrigin, float wanderRadius)
    {
        commonSetup(selfOb, playerOb);
        canWander = true;
        wandering = true;

        CanEngage = false;
        engaging = false;

        wanderAreaOrigin = wanderOrigin;
        wanderAreaRadius = wanderRadius;
        dest = getRandomDest();
        self.transform.position = dest;
    }

    //initialize so player CAN wander CAN engage
    public NPCMovement(GameObject selfOb, GameObject playerOb, Vector3 wanderOrigin, float wanderRadius, float maxEngagementDistance)
    {
        commonSetup(selfOb, playerOb);

        canWander = true;
        wandering = true;

        CanEngage = true;
        engaging = false;


        engagementRadius = maxEngagementDistance;

        wanderAreaOrigin = wanderOrigin;
        wanderAreaRadius = wanderRadius;
        dest = getRandomDest();
        self.transform.position = dest;

    }

    //initialize so player CANNOT wander CAN engage
    public NPCMovement(GameObject selfOb, GameObject playerOb, float maxEngagementDistance)
    {
        commonSetup(selfOb, playerOb); 

        canWander = false;
        wandering = false;

        CanEngage = true;
        engaging = false;


        engagementRadius = maxEngagementDistance;

        //default
        dest = new Vector3(0, 0, 0);
        wanderAreaOrigin = new Vector3(0, 0, 0);
    }

    //called by all constructors
    private void commonSetup(GameObject selfOb, GameObject playerOb)
    {
        player = playerOb;
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;

        lingerLength = getRandomPause();
    }


    public void UpdateMovement()
    {
        if (!agent.enabled)
            agent.enabled = true;

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

            if (FollowingPlayer || stickingAround)
            {
                follow();
            }
        
        if(stickingAround)
        {
            stickAroundCount++;
            if(stickAroundCount > stickAroundMax*60)
            {
                stickingAround = false;
                stickAroundCount = 0;
                ResumeWandering();
                Debug.Log("wander!!"); 
            }
        }

    }

    private void engage()
    {
        float distFromPlayer = getXZDist(player.transform.position, self.transform.position);
        SetWandering(false);
        if (distFromPlayer < bufferRadius)
        {
            if (distFromPlayer < tooCloseRadius)
            {
                Vector3 targetDirection = self.transform.position - player.transform.position;
                Vector3 newDest = self.transform.position + targetDirection.normalized * 10;
                dest = getRandomDest(newDest, 1f);
                GoHere(dest);
                agent.speed *= 2;
            }
            else
            {
                Chill();
            }
        }
        else if (distFromPlayer < engagementRadius)
            GoHere(player.transform.position);
        else
            SetEngaging(false); 
    }

    private void follow()
    {
        float distFromPlayer = Vector3.Distance(player.transform.position, self.transform.position);
        bool atDest = getXZDist(self.transform.position, dest) <= bufferDist;
        if (distFromPlayer > followDist * 1.3)
        {
            agent.isStopped = false;
            dest = getRandomDest(player.transform.position, followDist);
            GoHere(dest);
        }
        else if (atDest || distFromPlayer > followDist)
        {
            Chill(); 
        }
    }

    private void wander()
    {
        float destDistFromPlayer = Vector3.Distance(player.transform.position, dest);
        if (AvoidsPlayer && destDistFromPlayer < avoidsPlayerRadius)
        {
            agent.isStopped = false;
            //Action = Actions.Walking;
            dest = getRandomDest();
        }

        bool atDest = getXZDist(self.transform.position, dest) <= bufferDist;
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

    public void RunToPlayer()
    {
        Action = Actions.Running;
        followDist = followDist/1.5f; 
        dest = player.transform.position;
        agent.speed = baseSpeed * 2;

        stickingAround = true; 
    }

    public void SetFollowingDist(float followDistance)
    {
        FollowingPlayer = true;
        followDist = followDistance;
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
        agent.speed = baseSpeed; 
    }

    //send NPC to location
    public void GoHere(Vector3 loc)
    {
        if (Action != Actions.Running)
            Action = Actions.Walking;
        agent.isStopped = false;
        agent.SetDestination(loc);
    }

    //make NPC start wandering again
    public void ResumeWandering()
    {
        if (CanWander)
        {
            SetWandering(true);
            dest = getRandomDest();
            GoHere(dest); 

            lingerLength = getRandomPause();
            pauseCount = 0;
            agent.speed = baseSpeed; 
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
