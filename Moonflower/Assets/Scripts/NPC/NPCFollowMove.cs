using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFollowMove : MonoBehaviour, IMovement, INPCMovement
{
    public Actions Action { get; set; } 
    public bool Jumping { get; set; }

    public bool Active { get; set; } = true; 
    public GameObject Target;
    public bool HoldGround { get; set; } = false; 

    public bool stunned;
    public bool swinging;

    //public GameObject player;
    GameObject self;

    NavMeshAgent agent;
    Vector3 destination;


    //default
    public float followDist = 5; //default
    float tooCloseRadius = 2.5f;

    float baseSpeed;
    float baseAngularSpeed; 

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    //initialize so player CANNOT wander CANNOT engage
    public NPCFollowMove(GameObject selfOb, GameObject targetOb)
    {
        commonSetup(selfOb, targetOb);


    }

    //initialize so player CAN wander CANNOT engage
    public NPCFollowMove(GameObject selfOb, GameObject targetOb, float followDistance)
    {
        commonSetup(selfOb, targetOb);

        followDist = followDistance;
    }

    //initialize so player CAN wander CANNOT engage
    public NPCFollowMove(GameObject selfOb, GameObject targetOb, float followDistance, float tooCloseDistance)
    {
        commonSetup(selfOb, targetOb);

        followDist = followDistance;
        tooCloseRadius = tooCloseDistance; 
    }


    //called by all constructors
    private void commonSetup(GameObject selfOb, GameObject targetOb)
    {
        self = selfOb;
        Target = targetOb;
        agent = self.GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed * 1.5f;
        baseAngularSpeed = agent.angularSpeed; 
        destination = selfOb.transform.position;
    }


    public void UpdateMovement()
    {
        if (Active)
        {
            if (!agent.enabled)
                agent.enabled = true;

            float distFromTarget = getXZDist(Target.transform.position, self.transform.position);
            if (distFromTarget < followDist)
            { 
                if (!HoldGround && distFromTarget < tooCloseRadius)
                {
                    agent.angularSpeed = 0; 
                    Vector3 targetDirection = self.transform.position - Target.transform.position;
                    Vector3 newDest = self.transform.position + targetDirection.normalized * 10;
                    destination = getRandomDest(newDest, 1f);
                    GoHere(destination);
                    agent.speed *= 2;
                }
                else
                {
                    agent.speed = baseSpeed;
                    agent.angularSpeed = baseAngularSpeed;
                    Action = Actions.Walking; 
                    Chill();

                    // Stay facing who we're following (only needed in combat)
                    Vector3 relative = Target.transform.position - agent.transform.position;
                    float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                    agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * 10);
                }
            }
            else
            {
                GoHere(Target.transform.position);
            }
        }
    }

    public void ResumeMovement()
    {
        Active = true; 
    }

    public void SetFollowingDist(float followDistance)
    {
        followDist = followDistance;
    }

    //make sure npc give player a bit of space 
    public void SetTooClose(float dist)
    {
        tooCloseRadius = dist; 
    }

    public void SetFollowArea(float followDistance, float tooClose)
    {
        followDist = followDistance;
        tooCloseRadius = tooClose; 
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
