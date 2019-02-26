﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovementController : MonoBehaviour, IMovement
{
    public Actions Action { get; set; }
    public bool Jumping { get; set; }

    public enum MoveState { follow, wander, wanderfollow, chill }
    public MoveState state = MoveState.chill;
    MoveState defaultState = MoveState.chill; 

    NPCWanderMove wander;
    NPCFollowMove follow;
    bool canFollow;
    bool canWander; 

    public bool stunned;
    public bool swinging;

    public GameObject Player;
    GameObject self;
    GameObject target; 
    private bool stickingAround = false;
    int stickAroundCount = 0;
    const int stickAroundMax = 10;

    bool gettingBack = false; 

    NavMeshAgent agent;
    float baseSpeed;

    float avoidsPlayerRadius = 10f;

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    /*
     *  THIS IS ALL INITIALIZATION METHODS
     */
    //initialize so player CANNOT wander CANNOT engage
    public NPCMovementController(GameObject selfOb, GameObject playerOb)
    {
        Player = playerOb; 
        commonSetup(selfOb);
        canFollow = false;
        canWander = false;
    }

    //called by all constructors
    private void commonSetup(GameObject selfOb)
    {
        self = selfOb;
        agent = self.GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;

    }

    public void Follow(GameObject target, float followDist, float tooClose) 
    {
        canFollow = true; 
        if(follow == null)
        {
            follow = new NPCFollowMove(self, target, followDist, tooClose); 
        }
        else 
        {
            follow.Target = target;
            follow.SetFollowArea(followDist, tooClose); 
        }
        state = MoveState.follow; 
    }
    public void Follow(GameObject target, float followDist) 
    {
        canFollow = true;
        if (follow == null)
        {
            follow = new NPCFollowMove(self, target, followDist);
        }
        else
        {
            follow.Target = target;
            follow.SetFollowingDist(followDist);
        }
        state = MoveState.follow;
    }
    public void FollowPlayer(float followDist)
    {
        canFollow = true;
        if (follow == null)
        {
            follow = new NPCFollowMove(self, Player, followDist);
        }
        else
        {
            follow.Target = Player;
            follow.SetFollowingDist(followDist);
        }
        state = MoveState.follow;
    }
    public void FollowPlayer(float followDist, float tooClose)
    {
        canFollow = true;
        if (follow == null)
        {
            follow = new NPCFollowMove(self, Player, followDist, tooClose);
        }
        else
        {
            follow.Target = Player;
            follow.SetFollowArea(followDist,tooClose);
        }
        state = MoveState.follow;
    }

    public void Wander(Vector3 origin, float wanderDistance)
    {
        canWander = true;
        if (wander == null)
        {
            wander = new NPCWanderMove(self, origin, wanderDistance);
        }
        else
        {
            wander.SetArea(origin, wanderDistance); 
        }
        state = MoveState.wander;
    }
    public void WanderFollow(GameObject followTarget, float maxDistAway)
    {
        canWander = true;
        canFollow = true; 
        if(wander == null)
        {
            wander = new NPCWanderMove(self, target.transform.position, maxDistAway);
        }
        else
        {
            wander.SetArea(target.transform.position, maxDistAway);
        }

        if(follow == null)
        {
            follow = new NPCFollowMove(self, followTarget, maxDistAway/2); 
        }
        else
        {
            follow.Target = followTarget;
            follow.SetFollowingDist(maxDistAway/2); 
        }
        target = followTarget; 
        state = MoveState.wanderfollow;
    }
    public void WanderFollowPlayer(float maxDistAway)
    {
        canWander = true;
        canFollow = true; 
        target = Player;
        if (wander == null)
        {
            wander = new NPCWanderMove(self, target.transform.position, maxDistAway);
        }
        else
        {
            wander.SetArea(target.transform.position, maxDistAway);
        }
        if (follow == null)
        {
            follow = new NPCFollowMove(self, target, maxDistAway/2);
        }
        else
        {
            follow.Target = target;
            follow.SetFollowingDist(maxDistAway/2);
        }
        state = MoveState.wanderfollow;
    }
    public void SetDefault(MoveState dfs)
    {
        defaultState = dfs; 
    }


    /*
     * 
     * 
     * 
     * These are functional methods
     * 
     */

    public void UpdateMovement()
    {
        if (!agent.enabled)
            agent.enabled = true;


        if (stunned || swinging)
        {
            Chill(); 
        }

        // Dont move if we're stunned or swinging our weapon
        switch (state)
        {
            case MoveState.wander:
                if (canWander)
                {
                    //Debug.Log("I'm wandering!!");
                    wander.UpdateMovement();
                    Action = wander.Action;
                }
                break;
            case MoveState.follow:
                if (canFollow)
                { 
                    //Debug.Log("I'm following");
                    follow.UpdateMovement();
                    Action = follow.Action; 
                }
                break;
            case MoveState.wanderfollow:
                if (canWander && canFollow)
                {
                    float maxDist = wander.wanderAreaRadius; 
                    if (DistanceFrom(target) < maxDist*1.3f && !gettingBack)
                    {

                        wander.SetOrigin(target.transform.position);
                        wander.UpdateMovement();
                        Action = wander.Action;
                    }
                    else 
                    {
                        follow.UpdateMovement();
                        Action = follow.Action;
                        gettingBack = DistanceFrom(target) > maxDist / 1.7; 
                        if(!gettingBack)
                        {
                            wander.ResumeMovement(); 
                        }
                    }
                }
                break;
            case MoveState.chill:
                //Debug.Log("I'm chilling");
                break;
            default:
                break;
        }

        if (stickingAround)
        {
            stickAroundCount++;
            if (stickAroundCount > stickAroundMax * 60)
            {
                state = defaultState; 
                stickingAround = false;
                stickAroundCount = 0;
            }
        }
            
        
    }

    //start wandering
    public bool Wander()
    {
        if (canWander)
        {
            state = MoveState.wander;
        }
        return canWander;
    }

    //start following
    public bool Follow()
    {
        if (canFollow)
        {
            state = MoveState.follow;
        }
        return canFollow;
    }

    //start wander following
    public bool WanderFollow()
    {
        if (canWander && target != null)
        {
            state = MoveState.wanderfollow;
        }
        return (canWander && target != null);
    }

    //stop moving
    public void Chill()
    {
        Action = Actions.Chilling;
        state = MoveState.chill;
        agent.isStopped = true;
        agent.speed = baseSpeed;
    }

    public void Reset()
    {
        state = defaultState;
        agent.speed = baseSpeed; 

    }

    public void RunToPlayer()
    {
        Action = Actions.Running;
        agent.speed = baseSpeed * 2;

        stickingAround = true;
        state = MoveState.follow;
        follow.SetFollowingDist(follow.followDist / 1.5f); 
    }
    private void WalkToPlayer()
    {
        Action = Actions.Walking;
        state = MoveState.follow;
        follow.SetFollowingDist(follow.followDist / 1.5f);
    }

    public float DistanceFrom(GameObject a)
    {
        return Mathf.Abs(a.transform.position.x - self.transform.position.x) + Mathf.Abs(a.transform.position.z - self.transform.position.z);

    }

    // Disable player combat controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }

}
