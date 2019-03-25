using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class LesserNPCController : MonoBehaviour, INPCController
{
    private GameObject player;
    public GameObject dialoguePanel;
    public Sprite icon { get; set; }

    public bool canSeePlayer = false;

    public bool canInspect = true;
    public bool canTalk = true;
    public bool canDistract = true;
    public bool canGift = true;
    [HideInInspector] public bool[] actionsAvailable { get; private set; }

    public string inspectText;

    public float engagementRadius = 15f;
    public float tooCloseRad = 4f;
    public float bufferDist = 5f;

    //private bool engaging = false;
    private NPCMovementController movement;
    public NPCCombatController combatController;
    public StealthDetection stealthDetection;

    private NavMeshAgent agent;
    private DialogueTrigger talkTrig;
    private PlayerController playerController;
    private FeedbackText feedback;

    private Vector3 startPosition;

    private void Awake()
    {
        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        movement = new NPCMovementController(gameObject, player,Constants.MOUSE_NAME);
        //movement.SetEngagementDistances(5, combatController.attackDistance + 0.5f, 1);

        combatController = GetComponent<NPCCombatController>();
        stealthDetection = GetComponent<StealthDetection>();

        playerController = LevelManager.current.player.GetComponent<PlayerController>();
        icon = new IconFactory().GetIcon(Constants.MOUSE_ICON);
    }

    void Start()
    {
        startPosition = transform.position;

        // Setup Movement
        //float walkRad = WalkArea.GetComponent<Renderer>().bounds.size.x;
        Vector3 walkOrigin = transform.position;

        combatController.npcMovement = movement;

        //talkTrig = new AmaruDialogueTrigger(DialoguePanel, Constants.AMARU_ICON);

        LevelManager.current.RegisterNPC(gameObject);
        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    void Update()
    {
        if (talkTrig != null)
        {
            //if (playerController.Playing)
            //{
                //talkTrig.Update();

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
        //}
        //else
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

    // Action Wheel Interactions
    public void Talk()
    {

    }
    public void Gift(string giftName)
    {
        if (new ItemLookup().IsFood(giftName))
        {
            displayFeedback("They loves the " + giftName + "!");
            combatController.Subdue(); 
        }
        else
        {
            displayFeedback("They has no use for " + giftName + "...");
        }
    }
    public void Distract(GameObject distractedBy)
    {
        movement.Distracted(distractedBy);
    }

    public void EndDistract()
    {
        movement.Reset();
    }

    public string Inspect()
    {
        return Constants.MOUSE_NAME; 
    }

    private void StartEngagement()
    {
        //engaging = true;

        if (talkTrig != null)
            if (!talkTrig.DialogueActive())
                talkTrig.StartDialogue();
    }

    private void displayFeedback(string text)
    {
        feedback.ShowText(text);
    }

    private void HandleOnAggroUpdated(bool aggroed, GameObject aggroTarget)
    {
        if (aggroed)
        {
            movement.Follow(aggroTarget, combatController.attackDistance, 0.5f);
            movement.SetHoldGround(true);
            //movement.player = combatController.combatTarget;
            //movement.Attacking = true;

            if (stealthDetection && stealthDetection.enabled == true)
            {
                stealthDetection.BecomeAlerted(aggroTarget);
                stealthDetection.enabled = false;
            }
                
        }
        else
        {
            movement.Reset();
            //movement.Attacking = false;

            if (stealthDetection)
                stealthDetection.enabled = true;
        }
    }
    private void HandleOnAwarenessUpdated(int newAwareness)
    {
    }

    private void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;

        // Subscribe to recieve OnAggroUpdated event
        if (combatController)
            combatController.OnAggroUpdated += HandleOnAggroUpdated;
        if (stealthDetection)
            stealthDetection.OnAwarenessUpdate += HandleOnAwarenessUpdated;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;

        // Unsubscribe from recieving OnAggroUpdated event
        if (combatController)
            combatController.OnAggroUpdated -= HandleOnAggroUpdated;
        if (stealthDetection)
            stealthDetection.OnAwarenessUpdate -= HandleOnAwarenessUpdated;
    }

    // Disable player combat controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        //enabled = !isPaused;
    }

    public static GameObject GetRootmostObjectInLayer(Transform gameObject, string layer, string layer2 = "")
    {
        Transform parent = gameObject.parent;

        if (parent == null)
            return null;

        while (parent != gameObject)
        {
            if (parent.gameObject.layer == LayerMask.NameToLayer(layer) || parent.gameObject.layer == LayerMask.NameToLayer(layer2))
                return parent.gameObject;
            else
                parent = parent.parent;
        }

        return null;
    }
}