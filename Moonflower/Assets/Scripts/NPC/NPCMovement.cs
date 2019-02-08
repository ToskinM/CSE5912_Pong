using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{

    public bool Wandering { get { return wandering; } } //currently wandering
    public bool CanWander { get { return canWander; } } //capable of wandering
    public bool CanEngage { get; set; } //capable of being talked to 
    public bool Engaging  { get { return engaging; } } //currently being talked to 

    GameObject player;
    GameObject self; 
    bool canWander;
    bool wandering;
    bool engaging; 

    NavMeshAgent agent;
    Vector3 dest;
    Vector3 wanderAreaOrigin;
    float wanderAreaRadius = 5f;

    const float bufferDist = .2f; //max dist from destination point before going somewhere else
    int pauseCount = 0; //keeps track of how long NPC has been chilling at destination
    int lingerLength; //length of pause 
    const int smallPause = 0, largePause = 4; //max and min pause lengths

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


    public void Update()
    {
        if (Wandering)
        {
            Debug.Log("Wandering"); 
            wander();
        }

        if (CanEngage && !Engaging)
        {
            float distFromPlayer = Vector3.Distance(player.transform.position, self.transform.position);
            SetEngaging( distFromPlayer <= engagementRadius );
        }

        if (Engaging)
        {
            engage();
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
                self.transform.Translate(targetDirection.normalized * agent.speed * 2 * Time.deltaTime);

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
        bool atDest = getXZDist(agent.transform.position, dest) <= bufferDist;
        if (atDest && !agent.isStopped)
        {
            agent.isStopped = true;
        }
        else if (atDest)
        {

            if (pauseCount == lingerLength)
            {
                agent.isStopped = false;
                dest = getRandomDest();
                agent.SetDestination(dest);

                lingerLength = getRandomPause();
                pauseCount = 0;
            }
            else
            {
                pauseCount++;
            }

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
        agent.isStopped = true;
    }

    //send NPC to location
    public void GoHere(Vector3 loc)
    {
        agent.isStopped = false;
        agent.SetDestination(loc); 
    }

    //make NPC start wandering again
    public void ResumeWandering()
    {
        if (canWander)
        {
            agent.isStopped = false;
            SetWandering(true);
            dest = getRandomDest();
            agent.SetDestination(dest);
            lingerLength = getRandomPause();
            pauseCount = 0;
        }
    }

    private Vector3 getRandomDest()
    {
        //Renderer planeRend = plane.GetComponent<Renderer>();
        //Vector3 extent = planeRend.bounds.extents;
        //float x = planeRend.bounds.center.x + Random.Range(-extent.x, extent.x);
        float x = wanderAreaOrigin.x + Random.Range(-wanderAreaRadius, wanderAreaRadius);
        //float z = planeRend.bounds.center.z + Random.Range(-extent.z, extent.z);
        float z = wanderAreaOrigin.z + Random.Range(-wanderAreaRadius, wanderAreaRadius);
        return new Vector3(x, 0, z); 
    }

    private int getRandomPause()
    {
        return Random.Range(smallPause*60, largePause*60); 
    }

    private float getXZDist(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z);
    
    }



}
