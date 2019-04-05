using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TejuController : MonoBehaviour, INPCController
{
    //private GameObject player;
    public GameObject dialoguePanel;
    public Sprite icon { get; set; }

    public bool canSeePlayer = false;

    public bool canInspect = true;
    public bool canTalk = true;
    public bool canDistract = true;
    public bool canGift = true;
    [HideInInspector] public bool[] actionsAvailable { get; private set; }

    private NPCMovementController movement;
    public TejuCombatController combatController;

    private NavMeshAgent agent;
    private DialogueTrigger talkTrig;
    private PlayerController playerController;
    private FeedbackText feedback;

    private Vector3 startPosition;

    public string inspectText;

    private void Awake()
    {
        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        movement = new NPCMovementController(gameObject, Constants.MOUSE_NAME);

        combatController = GetComponent<TejuCombatController>();

        //playerController = PlayerController.instance.gameObject.GetComponent<PlayerController>();

        icon = new IconFactory().GetIcon(Constants.MOUSE_ICON);
    }

    void Start()
    {
        startPosition = transform.position;

        // Setup Movement
        Vector3 walkOrigin = transform.position;

        LevelManager.current.RegisterNPC(gameObject);
        //feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    void Update()
    {
        movement.UpdateMovement();
    }

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
            displayFeedback("They have no use for " + giftName + "...");
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
            //movement.Follow(aggroTarget, combatController.attackDistance, 0.5f);
            movement.SetHoldGround(true);
        }
        else
        {
            movement.Reset();
        }
    }
    private void HandleOnAwarenessUpdated(int newAwareness)
    {
    }

    private void OnEnable()
    {
        // Subscribe to recieve OnAggroUpdated event
        if (combatController)
            combatController.OnAggroUpdated += HandleOnAggroUpdated;
    }
    private void OnDisable()
    {
        // Unsubscribe from recieving OnAggroUpdated event
        if (combatController)
            combatController.OnAggroUpdated -= HandleOnAggroUpdated;
    }
}
