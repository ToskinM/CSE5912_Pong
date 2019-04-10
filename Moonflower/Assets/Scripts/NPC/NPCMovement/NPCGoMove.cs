using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCGoMove : MonoBehaviour, IMovement, INPCMovement
{
    public Actions Action { get; set; }
    public bool Jumping { get; set; }

    public bool Active { get; set; }
    public bool There { get; private set; } = false;
    public bool stunned;
    public bool swinging;

    public GameObject target;
    GameObject self;
    //bool canWander;

    NavMeshAgent agent;
    Vector3 destination;

    float bufferDist = 0.1f; 
    public float baseSpeed; 

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    //initialize so player CANNOT wander CANNOT engage
    public NPCGoMove(GameObject selfOb)
    {
        Active = true; 
        commonSetup(selfOb);

        //default
        destination = new Vector3(0, 0, 0);

    }

    //initialize so player CAN wander CANNOT engage
    public NPCGoMove(GameObject selfOb, Vector3 loc)
    {
        Active = true; 
        commonSetup(selfOb);

        destination = loc;
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
                Chill();
                There = true;
            }
            GoHere(destination);
        }
    }

    //make NPC start wandering again
    public void ResumeMovement()
    {
        Active = true;
        GoHere(destination);
    }

    public void SetLoc(Vector3 loc)
    {
        destination = loc;
        There = false; 
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
