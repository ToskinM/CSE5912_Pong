using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class LesserNPCController : MonoBehaviour, INPCController
{
    public GameObject Player;
    //public GameObject WalkArea;
    public GameObject DialoguePanel;
    public Sprite Icon { get; set; }

    public float engagementRadius = 15f;
    public float tooCloseRad = 4f;
    public float bufferDist = 5f;

    //private bool engaging = false;
    private NPCMovementController movement;
    public NPCCombatController combatController;
    private NavMeshAgent agent;
    private DialogueTrigger talkTrig;
    private IPlayerController playerController;

    private void Awake()
    {
        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<NPCCombatController>();
        playerController = Player.GetComponent<IPlayerController>();
        movement = new NPCMovementController(gameObject, Player);
        //movement.SetEngagementDistances(5, combatController.attackDistance + 0.5f, 1);
    }

    void Start()
    {
        // Setup Movement
        //float walkRad = WalkArea.GetComponent<Renderer>().bounds.size.x;
        Vector3 walkOrigin = transform.position;

        combatController.npcMovement = movement;

        //talkTrig = new AmaruDialogueTrigger(DialoguePanel, Constants.AMARU_ICON);

        LevelManager.current.RegisterNPC(gameObject);
    }

    void Update()
    {
        if (talkTrig != null)
        {
            if (playerController.Playing)
            {
                talkTrig.Update();

                //if (!talkTrig.Complete)
                //{
                //    StartEngagement();
                //}
                //else if (!movement.Wandering)
                //{
                //    movement.ResumeWandering();
                //    if (talkTrig.DialogueActive())
                //    {
                //        talkTrig.EndDialogue();
                //    }
                //}
            }
        }
        else
        {
            //if (combatController.inCombat)
            //{
            //    movement.player = combatController.combatTarget;
            //    movement.Attacking = true;
            //}
            //else
            //{
            //    movement.Attacking = false;
            //}
        }

        movement.UpdateMovement();
    }

    public void Talk()
    {

    }
    public void Gift(string giftName)
    {

    }
    public void Distract()
    {

    }
    public void Inspect()
    {

    }

    private void StartEngagement()
    {
        //engaging = true;

        if (talkTrig != null)
            if (!talkTrig.DialogueActive())
                talkTrig.StartDialogue();
    }

    private void HandleOnAggroUpdated(bool aggroed, GameObject aggroTarget)
    {
        if (aggroed)
        {
            movement.Follow(aggroTarget, combatController.attackDistance, 0.5f);
            movement.SetHoldGround(true); 
            //movement.player = combatController.combatTarget;
            //movement.Attacking = true;
        }
        else
        {
            movement.Reset();
            //movement.Attacking = false;
        }
    }

    private void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;

        // Subscribe to recieve OnAggroUpdated event
        if (combatController)
            combatController.OnAggroUpdated += HandleOnAggroUpdated;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;

        // Unsubscribe from recieving OnAggroUpdated event
        if (combatController)
            combatController.OnAggroUpdated -= HandleOnAggroUpdated;
    }

    // Disable player combat controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        //enabled = !isPaused;
}
}