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

    public NPCMovementController movement { get; set; }
    public TejuCombatController combatController;
    public TejuAnimationController animationtController;

    private NavMeshAgent agent;
    private DialogueTrigger talkTrig;
    private PlayerController playerController;
    private FeedbackText feedback;

    private Vector3 startPosition;

    public string inspectText;

    public SoulCrystal soulCrystal;
    public GameObject soulCrystalBarrier;
    public SpawnerController spawnerController;

    private void Awake()
    {
        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        movement = new NPCMovementController(gameObject, Constants.TEJU_NAME);

        combatController = GetComponent<TejuCombatController>();
        animationtController = GetComponent<TejuAnimationController>();

        //playerController = PlayerController.instance.gameObject.GetComponent<PlayerController>();

        icon = new IconFactory().GetIcon(Constants.TEGU_ICON);
    }

    void Start()
    {
        spawnerController.DeactivateAll();
        startPosition = transform.position;

        // Setup Movement
        Vector3 walkOrigin = transform.position;

        LevelManager.current.RegisterNPC(gameObject);
        //feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };

        animationtController.SetSleeping(true);
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
        if(new ItemLookup().IsSweet(giftName))
        {
            displayFeedback("Teju loves "+ giftName+" and has stopped crying!");
            combatController.Subdue();
        }
        else
        {
            displayFeedback("Teju is too sad for the " + giftName + ".");
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
        return Constants.TEJU_NAME;
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
            animationtController.SetSleeping(false);
            GetComponent<NPCSoundEffect>().NPCStop("Snoring");
            spawnerController.ActivateAll();
        }
        else
        {
            animationtController.SetSleeping(true);
            movement.Reset();
            spawnerController.DeactivateAll();
        }
    }
    private void HandleOnSubdue()
    {
        Debug.Log("Teju has been Subdued");
        soulCrystal.blocked = false;
        soulCrystalBarrier.SetActive(false);
    }

    private void OnEnable()
    {
        // Subscribe to recieve OnAggroUpdated event
        if (combatController)
        {
            combatController.OnAggroUpdated += HandleOnAggroUpdated;
            combatController.OnSubdue += HandleOnSubdue;
        }
    }
    private void OnDisable()
    {
        // Unsubscribe from recieving OnAggroUpdated event
        if (combatController)
        {
            combatController.OnAggroUpdated -= HandleOnAggroUpdated;
            combatController.OnSubdue -= HandleOnSubdue;
        }
    }
}
