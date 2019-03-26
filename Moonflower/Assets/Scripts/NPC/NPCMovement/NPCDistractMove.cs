using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCDistractMove : MonoBehaviour, IMovement, INPCMovement
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

    float baseSpeed; 

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
        //agent = self.GetComponent<NavMeshAgent>();
        agent = GetComponent<NavMeshAgent>();
    }

    //initialize so player CANNOT wander CANNOT engage
    public NPCDistractMove(GameObject selfOb)
    {
        Active = true; 
        commonSetup(selfOb);

        //default

    }

    //initialize so player CAN wander CANNOT engage
    public NPCDistractMove(GameObject selfOb, GameObject targetOb)
    {
        Active = true; 
        commonSetup(selfOb);

        target = targetOb;
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
            self.transform.LookAt(target.transform.position);
            agent.speed = baseSpeed;
            agent.isStopped = true; 

        }
    }

    //make NPC start wandering again
    public void ResumeMovement()
    {
    }

    public void SetTarget(GameObject tar)
    {
        target = tar; 
    }

    //stop NPC movement
    public void Chill()
    {
        Action = Actions.Chilling;
        if (agent == null)
            Debug.Log("no agent");
        agent.isStopped = true;
    }

    //send NPC to location
    public void GoHere(Vector3 loc)
    {
    }


    // Disable player combat controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }

}
