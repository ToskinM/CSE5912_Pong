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
    private PlayerController playerController;
    private FeedbackText feedback;

    private Vector3 startPosition;

    public string inspectText;

    public SoulCrystal soulCrystal;
    public GameObject soulCrystalBarrier;
    public SpawnerController spawnerController;

    private DialogueTrigger start;
    private DialogueTrigger rep;
    private DialogueTrigger currTalk;
    enum Convo { start, rep }
    Convo currConvo;

    bool subdued = false;
    bool aboutToReset = false; 

    private void Awake()
    {
        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        movement = new NPCMovementController(gameObject, Constants.TEJU_NAME);

        combatController = GetComponent<TejuCombatController>();
        animationtController = GetComponent<TejuAnimationController>();

        //playerController = PlayerController.instance.gameObject.GetComponent<PlayerController>();

        icon = new IconFactory().GetIcon(Constants.TEJU_ICON);
        start = new DialogueTrigger(gameObject, icon, Constants.TEJU_START_DIALOGUE);
        start.SetExitText("I'm not even worth a full conversation...");
        rep = new DialogueTrigger(gameObject, icon, Constants.TEJU_REP_DIALOGUE);
        rep.SetExitText("I'm not even worth a full conversation...");

        if (!GameStateController.current.NPCDialogues.ContainsKey(Constants.TEJU_NAME))
        {
            currTalk = start;
            currConvo = Convo.start; 
            GameStateController.current.SaveNPCDialogues(Constants.TEJU_NAME, currConvo.ToString(), currTalk);
        }
        else
        {
            currTalk = GameStateController.current.GetNPCDialogue(Constants.TEJU_NAME);
            currTalk.SetSelf(gameObject);
            string convo = GameStateController.current.GetNPCDiaLabel(Constants.TEJU_NAME);
            if (convo.Equals(Convo.start.ToString()))
            {
                currConvo = Convo.start;
                start = currTalk; 
            }
            else
            {
                currConvo = Convo.rep;
                rep = currTalk;
            }

            GameStateController.current.SaveNPCDialogues(Constants.TEJU_NAME, currConvo.ToString(), currTalk);

        }
    }

    void Start()
    {
        spawnerController.DeactivateAll();
        startPosition = transform.position;

        // Setup Movement
        Vector3 walkOrigin = transform.position;

        LevelManager.current.RegisterNPC(gameObject);
        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };

        animationtController.SetSleeping(true);
    }

    void Update()
    {
        movement.UpdateMovement();
        currTalk.Update(); 
        if(currConvo == Convo.start && start.Complete && !start.panelInfo.IsUp)
        {
            currConvo = Convo.rep;
            currTalk = rep;
            GameStateController.current.SaveNPCDialogues(Constants.TEJU_NAME, currConvo.ToString(), currTalk);

        }

        if (currTalk.Complete && !aboutToReset && !subdued)
        {
            aboutToReset = true;
            Invoke("ResetConvo", 2f);
        }

    }

    public void Talk()
    {
        if (currTalk.Complete)
        {
            if(subdued)
                displayFeedback("Teju invited you to touch the soul crystal.");
            else
                displayFeedback("Teju is still devastated.");
        }
        else
        {
            StartEngagement();
        }
    }
    public void Gift(string giftName)
    {
        if(giftName.Equals(Constants.HONEY_NAME))
        {
            displayFeedback("Teju loves the " + giftName + " and has stopped crying!");
            GameStateController.current.dialogueEvents.IncreasePlayerCharisma(true);
            Subdue();
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

    public void Subdue()
    {
        if (!feedback.IsDisplaying())
            displayFeedback("Teju has stopped crying!");

        combatController.Subdue();
        subdued = true; 
    }

    public void FailConvo()
    {
        StartCoroutine(combatController.AreaCryAttack());
        if (currConvo == Convo.rep)
        {
            aboutToReset = true; 
            Invoke("ResetConvo", 2f);
        }
    }

    private void ResetConvo()
    {
        currTalk.Reset();
        aboutToReset = false;
    }

    private void StartEngagement()
    {
        if (!currTalk.DialogueActive())
            currTalk.StartDialogue();
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
