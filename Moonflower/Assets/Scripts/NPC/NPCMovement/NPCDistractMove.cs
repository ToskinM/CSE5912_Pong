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
    Vector3 destination;

    float baseSpeed; 

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    //initialize so player CANNOT wander CANNOT engage
    public NPCDistractMove(GameObject selfOb)
    {
        Active = true; 
        commonSetup(selfOb);

        //default
        destination = new Vector3(0, 0, 0);

    }

    //initialize so player CAN wander CANNOT engage
    public NPCDistractMove(GameObject selfOb, GameObject targetOb)
    {
        Active = true; 
        commonSetup(selfOb);

        target = targetOb;
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
            Vector3 relative = target.transform.position - agent.transform.position;
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * 10);

            agent.speed = baseSpeed; 

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
