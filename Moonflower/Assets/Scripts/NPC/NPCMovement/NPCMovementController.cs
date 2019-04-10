using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovementController : MonoBehaviour, IMovement
{
    public Actions Action { get; set; }
    public bool Jumping { get; set; }
    public bool Active = true; 

    public enum MoveState { follow, wander, wanderfollow, pace, chill, distractChill, go }
    public MoveState state = MoveState.chill;
    MoveState defaultState = MoveState.chill; 
    public bool IsDefault { get { return state == defaultState; } }

    NPCWanderMove wander;
    NPCFollowMove follow;
    NPCPaceMove pace;
    NPCDistractMove distract;
    NPCGoMove go; 
    bool canFollow;
    bool canWander;
    bool canPace;
    bool canBeDistracted;
    bool canGo; 
    bool running = false; 

    public bool stunned;
    public bool swinging;

    //public GameObject Player;


    GameObject self;
    GameObject target; 
    private bool stickingAround = false;
    int stickAroundCount = 0;
    const int stickAroundMax = 10;

    bool gettingBack = false; 

    NavMeshAgent agent;
    float baseSpeed;

    float avoidsPlayerRadius = 10f;
    float gettingClose = 5f;
    string charName;

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
        distract = GetComponent<NPCDistractMove>();
    }

    /*
     *  THIS IS ALL INITIALIZATION METHODS
     */
    //initialize so player CANNOT wander CANNOT engage
    public NPCMovementController(GameObject selfOb, string charname)
    {
        charName = charname; 
        //Player = playerOb; 
        commonSetup(selfOb);
        canFollow = false;
        canWander = false;
        canPace = false;
        canGo = false; 
        canBeDistracted = false; 
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
        if(!canFollow)
        {
            follow = new NPCFollowMove(self, target, followDist, tooClose);
            canFollow = true;
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
        if (!canFollow)
        {
            follow = new NPCFollowMove(self, target, followDist);
            canFollow = true;
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
        if (!canFollow)
        {
            follow = new NPCFollowMove(self, PlayerController.instance.GetActivePlayerObject(), followDist);
            canFollow = true;
        }
        else
        {
            follow.Target = PlayerController.instance.GetActivePlayerObject();
            follow.SetFollowingDist(followDist);
        }
        state = MoveState.follow;
    }
    public void FollowPlayer(float followDist, float tooClose)
    {
        if (!canFollow)
        {
            follow = new NPCFollowMove(self, PlayerController.instance.GetActivePlayerObject(), followDist, tooClose);
            canFollow = true;
        }
        else
        {
            follow.Target = PlayerController.instance.GetActivePlayerObject();
            follow.SetFollowArea(followDist,tooClose);
        }
        state = MoveState.follow;
    }

    public void Pace(Vector3 origin, float distance)
    {
        if (!canPace)
        {
            pace = new NPCPaceMove(self, origin, distance);
            canPace = true;
        }
        else
        {
            pace.SetPath(origin, distance);
        }
        state = MoveState.pace;
    }
    public void Go(Vector3 origin)
    {
        if (!canGo)
        {
            go = new NPCGoMove(self, origin);
            canGo = true;
        }
        else
        {
            go.SetLoc(origin);
        }
        state = MoveState.go;
        //        Debug.Log(charName + " set wander"); 
    }
    public void Distracted(GameObject targetObject)
    {
        if (!canBeDistracted)
        {
            distract = new NPCDistractMove(self, targetObject);
            canBeDistracted = true;
        }
        else
        {
            distract.SetTarget(targetObject);
        }
        //distract.SetTarget(target);
        state = MoveState.distractChill;

    }
    public void Wander(Vector3 origin, float wanderDistance)
    {
        if (!canWander)
        {
            wander = new NPCWanderMove(self, origin, wanderDistance);
            canWander = true;
        }
        else
        {
            wander.SetArea(origin, wanderDistance); 
        }
        state = MoveState.wander;
//        Debug.Log(charName + " set wander"); 
    }
    public void WanderFollow(GameObject followTarget, float maxDistAway)
    {
         
        if(!canWander)
        {
            wander = new NPCWanderMove(self, followTarget.transform.position, maxDistAway);
            canWander = true;
        }
        else
        {
            wander.SetArea(followTarget.transform.position, maxDistAway);
        }

        if(!canFollow)
        {
            follow = new NPCFollowMove(self, followTarget, maxDistAway/2);
            canFollow = true;
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
        target = PlayerController.instance.GetActivePlayerObject();
        if (!canWander)
        {
            wander = new NPCWanderMove(self, target.transform.position, maxDistAway);
            canWander = true;
        }
        else
        {
            wander.SetArea(target.transform.position, maxDistAway);
        }
        if (!canFollow)
        {
            follow = new NPCFollowMove(self, target, maxDistAway/2);
            canFollow = true;
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
    public void SetLoc(Vector3 pos)
    {
        self.transform.position = pos;
        state = defaultState;
    }
    public void SetHoldGround(bool hold)
    {
        if (canFollow)
            follow.HoldGround = hold; 
    }
    public void InfluenceWanderSpeed(float mult)
    {
        if (canWander)
            wander.baseSpeed *= mult; 
    }
    public void InfluenceFollowSpeed(float mult)
    {
        if (canFollow)
            follow.baseSpeed *= mult;
    }
    public void InfluencePaceSpeed(float mult)
    {
        if (canPace)
            pace.baseSpeed *= mult;
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
        if (!self.activeSelf || !agent.isActiveAndEnabled) return; 
        if (Active)
        {
            //Debug.Log(charName + " speed " + agent.speed + " with base " + baseSpeed);
            if (!agent.enabled)
                agent.enabled = true;


            if (stunned || swinging)
            {
                Chill();
            }

            if (agent.speed < baseSpeed)
                agent.speed = baseSpeed; 

            // Dont move if we're stunned or swinging our weapon
            switch (state)
            {
                case MoveState.wander:
//                    Debug.Log(charName + " wander");
                    if (canWander)
                    {
//                        Debug.Log(charName + " is wandering!!");
                        wander.UpdateMovement();
                        Action = wander.Action;

                    }
                    break;
                case MoveState.pace:
                    if (canPace)
                    {
                        //Debug.Log("I'm wandering!!");
                        pace.UpdateMovement();
                        Action = follow.Action;
                    }
                    break;
                case MoveState.go:
                    if (canGo)
                    {
                        //Debug.Log("I'm wandering!!");
                        go.UpdateMovement();
                        Action = follow.Action;
                        if(go.There)
                        {
                            state = MoveState.chill; 
                        }
                    }
                    break;
                case MoveState.follow:
//                    Debug.Log(charName + " follow");
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
                        if (DistanceFrom(target) < maxDist * 1.3f && !gettingBack && !stickingAround)
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
                            if (!gettingBack && !stickingAround)
                            {
                                wander.ResumeMovement();
                            }
                        }
                    }
                    break;
                case MoveState.distractChill:
                    //Debug.Log("distract"); 
                    if(canBeDistracted)
                    {
                        distract.UpdateMovement();
                        Action = Actions.Chilling;
                    }
                    //agent.isStopped = true;
                    //Action = Actions.Chilling;
                    //self.transform.LookAt(target.transform.position);
                    break;
                case MoveState.chill:
  //                  Debug.Log(charName + " chill");
                    //Chill(); 
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
                    Reset(); 
                    stickingAround = false;
                    stickAroundCount = 0;
                }
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
    //start following
    public bool Pace()
    {
        if (canPace)
        {
            state = MoveState.pace;
        }
        return canPace;
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
        switch (state)
        {
            case MoveState.wander:
                wander.Chill();
                break;
            case MoveState.wanderfollow:
                wander.Chill();
                follow.Chill();
                break;
            case MoveState.follow:
                follow.Chill();
                break;
        }
        state = MoveState.chill;
        agent.isStopped = true;
        agent.speed = baseSpeed;
        //Debug.Log(charName+" chill");
    }

    public void GoToLoc(GameObject loc)
    {
        Go(loc.transform.position);  
    }

    public void Reset()
    {
        //Debug.Log(charName + " reset"); 
        state = defaultState;
        agent.speed = baseSpeed;
        SetHoldGround(false); 
    }

    public void Run(float num = 1)
    {
        //Debug.Log(charName + " Run boi run at " + baseSpeed * 2 * num); 
        follow.Action = Actions.Running;
        agent.speed = baseSpeed * 2*num;
        switch(state)
        {
            case MoveState.wander:
                wander.Run();
                break;
            case MoveState.wanderfollow:
                wander.Run();
                follow.Run();
                break;
            case MoveState.follow:
                follow.Run();
                break;
        }
    }
    

    public void RunToPlayer()
    {
        Action = Actions.Running;
        agent.speed = baseSpeed * 2;
        follow.Target = PlayerController.instance.GetActivePlayerObject(); 

        stickingAround = true;
        if(state != MoveState.wanderfollow)
            state = MoveState.follow; 
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
        //enabled = !isPaused;
    }

}
