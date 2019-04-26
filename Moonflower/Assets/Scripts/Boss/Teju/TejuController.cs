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

    //private NavMeshAgent agent;
    private PlayerController playerController;
    private FeedbackText feedback;

    private Vector3 startPosition;

    public string inspectText;

    public SoulCrystal soulCrystal;
    public GameObject soulCrystalBarrier;
    public SpawnerController spawnerController;

    NPCDialogueEvents diaEvent; 

    private DialogueTrigger startPeace;
    private DialogueTrigger startAttack;
    private DialogueTrigger startAttackWeap;
    private DialogueTrigger repPeace;
    private DialogueTrigger repAttack;
    private DialogueTrigger repAttackWeap;
    private DialogueTrigger currTalk;
    enum Convo { start, rep }
    Convo currConvo;

    bool subdued = false;
    bool aboutToReset = false; 

    private void Awake()
    {
        // Initialize Components
        //agent = GetComponent<NavMeshAgent>();
        //movement = new NPCMovementController(gameObject, Constants.TEJU_NAME);

        combatController = GetComponent<TejuCombatController>();
        animationtController = GetComponent<TejuAnimationController>();
        diaEvent = GameStateController.current.gameObject.GetComponent<NPCDialogueEvents>();

        //playerController = PlayerController.instance.gameObject.GetComponent<PlayerController>();

        icon = new IconFactory().GetIcon(Constants.TEJU_ICON);
        startPeace = new DialogueTrigger(gameObject, icon, Constants.TEJU_START_PEACE_DIALOGUE);
        startPeace.SetExitText("I'm not even worth a full conversation...");
        startAttack = new DialogueTrigger(gameObject, icon, Constants.TEJU_START_ATTACK_DIALOGUE);
        startAttack.SetExitText("I'm not even worth a full conversation...");
        startAttackWeap = new DialogueTrigger(gameObject, icon, Constants.TEJU_START_ATTACKWEAP_DIALOGUE);
        startAttackWeap.SetExitText("I'm not even worth a full conversation...");
        repPeace = new DialogueTrigger(gameObject, icon, Constants.TEJU_REP_PEACE_DIALOGUE);
        repPeace.SetExitText("I'm not even worth a full conversation...");
        repAttack = new DialogueTrigger(gameObject, icon, Constants.TEJU_REP_ATTACK_DIALOGUE);
        repAttack.SetExitText("I'm not even worth a full conversation...");
        repAttackWeap = new DialogueTrigger(gameObject, icon, Constants.TEJU_REP_ATTACKWEAP_DIALOGUE);
        repAttackWeap.SetExitText("I'm not even worth a full conversation...");

        if (!GameStateController.current.NPCDialogues.ContainsKey(Constants.TEJU_NAME))
        {
            currTalk = startPeace;
            currConvo = Convo.start; 
            GameStateController.current.SaveNPCDialogues(Constants.TEJU_NAME, currConvo.ToString(), currTalk);
        }
        else
        {
            string convo = GameStateController.current.GetNPCDiaLabel(Constants.TEJU_NAME);
            if (convo.Equals(Convo.start.ToString()))
            {
                currConvo = Convo.start;
                currTalk = startPeace; 
            }
            else
            {
                currConvo = Convo.rep;
                currTalk = repPeace; 
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
        //movement.UpdateMovement();
        currTalk.Update(); 
        if(currConvo == Convo.start && currTalk.Complete && !currTalk.panelInfo.IsUp)
        {
            currConvo = Convo.rep;
            currTalk = repPeace;
            GameStateController.current.SaveNPCDialogues(Constants.TEJU_NAME, currConvo.ToString(), currTalk);

        }

        if (currTalk.Complete && !aboutToReset && !subdued)
        {
            FailConvo(); 
        }

    }

    public void Talk()
    {
        if (currTalk.Complete)
        {
            if(subdued)
                displayFeedback("Teju invited you to touch the soul crystal.");
            else
                displayFeedback("Teju is still devastated and crying.");
        }
        else
        {
            switch(currConvo)
            {
                case Convo.start:
                    if(!diaEvent.WasMorePeaceful() || !diaEvent.IsNotArmed())
                    {
                        if(diaEvent.IsNotArmed())
                        {
                            currTalk = startAttack; 
                        }
                        else
                        {
                            currTalk = startAttackWeap;
                        }

                    }
                    break;
                case Convo.rep:
                    if (!diaEvent.WasMorePeaceful() || !diaEvent.IsNotArmed())
                    {
                        if (diaEvent.IsNotArmed())
                        {
                            currTalk = repAttack;
                        }
                        else
                        {
                            currTalk = repAttackWeap;
                        }

                    }
                    break;

            }

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
        //movement.Distracted(distractedBy);
    }

    public void EndDistract()
    {
        //movement.Reset();
    }

    public string Inspect()
    {
        return Constants.TEJU_NAME;
    }

    public void Subdue()
    {
        //animationtController.SetTalking(false);
        if (!feedback.IsDisplaying())
            displayFeedback("Teju has stopped crying!");

        combatController.Subdue();
        subdued = true;

        startPeace.SetExitText("Thank you for helping me connect to my soul crystal.");
        startAttack.SetExitText("Thank you for helping me connect to my soul crystal.");
        startAttackWeap.SetExitText("Thank you for helping me connect to my soul crystal.");
        repPeace.SetExitText("Thank you for helping me connect to my soul crystal.");
        repAttack.SetExitText("Thank you for helping me connect to my soul crystal.");
        repAttackWeap.SetExitText("Thank you for helping me connect to my soul crystal.");
    }

    public void FailConvo()
    {
        //Debug.Log("end");
        //animationtController.SetTalking(false);

        if (combatController.areaCryCoroutine == null)
            combatController.areaCryCoroutine = StartCoroutine(combatController.AreaCryAttack());

        if (currConvo == Convo.rep)
        {
            aboutToReset = true; 
            Invoke("ResetConvo", 2f);
        }
    }

    private void ResetConvo()
    {
        currTalk.Reset(); 
        currTalk = repPeace; 
        aboutToReset = false;
    }

    private void StartEngagement()
    {
        animationtController.SetTalking(true);
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
            //movement.SetHoldGround(true);
            animationtController.SetSleeping(false);
            GetComponent<NPCSoundEffect>().NPCStop("Snoring");
            spawnerController.ActivateAll();
        }
        else
        {
            animationtController.SetSleeping(true);
            //movement.Reset();
            spawnerController.DeactivateAll();
        }
    }
    private void HandleOnSubdue()
    {
        //Debug.Log("Teju has been Subdued");
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
