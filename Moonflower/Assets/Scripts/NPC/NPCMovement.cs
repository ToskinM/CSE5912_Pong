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
    public bool Attacking { get { return attacking; } set { attacking = value; } } //currently persuing a combat target 
    public bool AvoidsPlayer { get; set; } = false;
    public bool FollowingPlayer { get; set; } = false;

    public bool stunned;

    public GameObject player;
    GameObject self;
    bool canWander;
    bool wandering;
    bool engaging;
    bool attacking;
    private bool stickingAround = false;
    int stickAroundCount = 0;
    int stickAroundMax = 10;

    NavMeshAgent agent;
    Vector3 destination;
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

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    //initialize so player CANNOT wander CANNOT engage
    public NPCMovement(GameObject selfOb, GameObject playerOb)
    {
        commonSetup(selfOb, playerOb);

        canWander = false;
        wandering = false;

        CanEngage = false;
        engaging = false;

        //default
        destination = new Vector3(0, 0, 0);
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
        destination = getRandomDest();
        self.transform.position = destination;
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
        destination = getRandomDest();
        self.transform.position = destination;

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
        destination = new Vector3(0, 0, 0);
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

        // Stop moving if we're stunned
        if (stunned)
        {
            Chill();
        }
        else
        {
            if (attacking)
            {
                Attack();
            }
            else
            {
                if (Wandering)
                {
                    Wander();
                }

                if (CanEngage)
                {
                    if (!Engaging)
                    {
                        if (player)
                        {
                            float distFromPlayer = Vector3.Distance(player.transform.position, self.transform.position);
                            SetEngaging(distFromPlayer <= engagementRadius);
                        }
                    }
                    if (Engaging)
                    {
                        if (player)
                            Engage();
                    }
                }

                if (FollowingPlayer || stickingAround)
                {
                    Follow();
                }

                if (stickingAround)
                {
                    stickAroundCount++;
                    if (stickAroundCount > stickAroundMax * 60)
                    {
                        stickingAround = false;
                        stickAroundCount = 0;
                        ResumeWandering();
                        Debug.Log("wander!!");
                    }
                }
            }
        }
    }

    private void Engage()
    {
        float distFromPlayer = getXZDist(player.transform.position, self.transform.position);
        SetWandering(false);
        if (distFromPlayer < bufferRadius)
        {
            if (distFromPlayer < tooCloseRadius)
            {
                Vector3 targetDirection = self.transform.position - player.transform.position;
                Vector3 newDest = self.transform.position + targetDirection.normalized * 10;
                destination = getRandomDest(newDest, 1f);
                GoHere(destination);
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

    private void Follow()
    {
        float distFromPlayer = Vector3.Distance(player.transform.position, self.transform.position);
        bool atDest = getXZDist(self.transform.position, destination) <= bufferDist;
        if (distFromPlayer > followDist * 1.3)
        {
            agent.isStopped = false;
            destination = getRandomDest(player.transform.position, followDist);
            GoHere(destination);
        }
        else if (atDest || distFromPlayer > followDist)
        {
            Chill();
        }
    }

    private void Attack()
    {
        float distFromPlayer = getXZDist(player.transform.position, self.transform.position);
        SetWandering(false);

        // If we're within buffer radius
        if (distFromPlayer < bufferRadius)
        {
            if (distFromPlayer < tooCloseRadius)
            {
                // Move back a bit
                Vector3 targetDirection = self.transform.position - player.transform.position;
                Vector3 newDest = self.transform.position + targetDirection.normalized * 10;
                destination = getRandomDest(newDest, 1f);
                GoHere(destination);
                agent.speed *= 2;
            }
            else
            {
                Chill();

                // look at target
                Vector3 relative = player.transform.position - self.transform.position;
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                //self.transform.rotation = Quaternion.Euler(0, angle, 0);
                self.transform.rotation = Quaternion.RotateTowards(self.transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * 300f);
            }
        }
        else
            GoHere(player.transform.position); // pursue combat target
    }

    private void Wander()
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

    public void RunToPlayer()
    {
        Action = Actions.Running;
        followDist = followDist / 1.5f;
        destination = player.transform.position;
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
        if (getXZDist(destination, loc) > wanderAreaRadius)
        {
            wanderAreaOrigin = loc;
            destination = getRandomDest();
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
            destination = getRandomDest();
            GoHere(destination);

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
