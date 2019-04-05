﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWanderMove : MonoBehaviour, IMovement, INPCMovement
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


    public Vector3 wanderAreaOrigin;
    public float wanderAreaRadius = 5f; //default
    float avoidsTargetRadius = 5f;
    const float avoidsPlayerRadius = 3f;
    float lastDist = 0f;
    int giveUpCount = 0;
    const int giveUpMax = 20;
    const float giveupDist = .2f; 

    const float bufferDist = .4f; //max dist from destination point before going somewhere else
    int pauseCount = 0; //keeps track of how long NPC has been chilling at destination
    int lingerLength; //length of pause 
    const int smallPause = 1, largePause = 4; //max and min pause lengths

    public float baseSpeed;
    float doubleSpeed; 

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    //initialize so player CANNOT wander CANNOT engage
    public NPCWanderMove(GameObject selfOb)
    {
        Active = true; 
        commonSetup(selfOb);

        //default
        destination = new Vector3(0, 0, 0);
        wanderAreaOrigin = new Vector3(0, 0, 0);

    }

    //initialize so player CAN wander CANNOT engage
    public NPCWanderMove(GameObject selfOb, Vector3 wanderOrigin, float wanderRadius)
    {
        Active = true; 
        commonSetup(selfOb);

        wanderAreaOrigin = wanderOrigin;
        wanderAreaRadius = wanderRadius;
        destination = getRandomDest();
        self.transform.position = destination;
    }

    //initialize so player CAN wander CANNOT engage
    public NPCWanderMove(GameObject selfOb, Vector3 wanderOrigin, float wanderRadius, GameObject avoidTarget)
    {
        target = avoidTarget; 
        Active = true;
        commonSetup(selfOb);

        wanderAreaOrigin = wanderOrigin;
        wanderAreaRadius = wanderRadius;
        destination = getRandomDest();
        self.transform.position = destination;
    }

    //called by all constructors
    private void commonSetup(GameObject selfOb)
    {
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();

        lingerLength = getRandomPause();
        baseSpeed = agent.speed;
        //doubleSpeed = baseSpeed * 2; 
    }


    public void UpdateMovement()
    {

        if (Active)
        {
            if (AvoidsTarget)
            {
                float distFromTarget = getXZDist(target.transform.position, self.transform.position);
                if (distFromTarget < avoidsTargetRadius)
                {
                    agent.isStopped = false;
                    backup();
                }
            }

            float distFromPlayer = getXZDist(PlayerController.instance.GetActivePlayerObject().transform.position, self.transform.position);
            if (!agent.isStopped && distFromPlayer < avoidsPlayerRadius)
            {
                agent.isStopped = false;
                backupFromPlayer();
            }

            //agent.speed = baseSpeed;
            float distFromDest = getXZDist(self.transform.position, destination);
            if (agent.speed > baseSpeed && distFromDest < 5f)
            {
                agent.speed /= 1.3f;
            }
            bool atDest = distFromDest <= bufferDist;
            if (atDest && !agent.isStopped)
            {
                Chill();
                Action = Actions.Chilling;
                agent.isStopped = true;
            }
            else if (atDest)
            {
                lastDist = distFromDest; 
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
                if(Mathf.Abs(distFromDest - lastDist) <= giveupDist)
                {
                    giveUpCount++; 
                    if(giveUpCount > giveUpMax)
                    {
                        Debug.Log("give up");
                        agent.Warp(self.transform.position - new Vector3(.2f, 0, .2f));
                        destination = getRandomDest();
                        GoHere(destination);
                        giveUpCount = 0; 
                    }
                }
                else
                {
                    giveUpCount = 0;
                }

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

    public void Run()
    {
        //agent.speed = doubleSpeed; 
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
        AvoidsTarget = true;
        avoidsTargetRadius = dist;
    }

    //stop NPC movement
    public void Chill()
    {
        Action = Actions.Chilling;
        agent.isStopped = true;
        //agent.speed = baseSpeed;
    }

    //send NPC to location
    public void GoHere(Vector3 loc)
    {
        if (Action != Actions.Running)
            Action = Actions.Walking;
        agent.isStopped = false;
        agent.SetDestination(loc);
    }

    private void backup()
    {

        Vector3 targetDirection = self.transform.position - target.transform.position;
        Vector3 newDest = self.transform.position + targetDirection.normalized * 10;
        destination = getRandomDest(newDest, 1f);
        GoHere(destination);
        //agent.speed *= 2;

    }

    private void backupFromPlayer()
    {

        Vector3 targetDirection = self.transform.position - PlayerController.instance.GetActivePlayerObject().transform.position;
        Vector3 newDest = self.transform.position + targetDirection.normalized * avoidsPlayerRadius;
        destination = getRandomDest(newDest, 1f);
        GoHere(destination);
        //agent.speed *= 2;

    }

    // always gets a reachable point on the navmesh
    private Vector3 getRandomDest()
    {
        return getRandomDest(wanderAreaOrigin, wanderAreaRadius);
    }

    // always gets a reachable point on the navmesh around origin 
    private Vector3 getRandomDest(Vector3 origin, float radius)
    {
        if (!agent.isActiveAndEnabled)
        {
            return self.transform.position; 
        }

        int count = 0;
        int bailCount = 20;

        float x, z;
        NavMeshHit hit = new NavMeshHit();
        bool viablePosition = true, viablePath = true, notTooClose = true;
        do
        {
            x = origin.x + Random.Range(-radius, radius);
            z = origin.z + Random.Range(-radius, radius);
            viablePosition = NavMesh.SamplePosition(new Vector3(x, self.transform.position.y, z), out hit, radius, NavMesh.AllAreas);
            viablePath = agent.CalculatePath(hit.position, new NavMeshPath());

            if (AvoidsTarget && viablePath)
            {
                notTooClose = getXZDist(hit.position, target.transform.position) > avoidsTargetRadius;
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
