using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;

public class PinonController : MonoBehaviour, INPCController
{
    //public GameObject Player;
    public GameObject WalkCenter;
    public GameObject DialoguePanel;
    public Sprite icon { get; set; }
    public bool dialogueActive = false;

    const float engagementRadius = 9f;
    const float tooCloseRad = 3f;
    const float bufferDist = 4f;
    const float wanderRad = 30f;

    public bool canInspect = true;
    public bool canTalk = true;
    public bool canDistract = true;
    public bool canGift = true;
    [HideInInspector] public bool[] actionsAvailable { get; private set; }

    NPCMovementController npc;
    Animator NPCController;
    NavMeshAgent agent;

    enum Convo { first, intro }
    Convo currConvo = Convo.first; 
    DialogueTrigger currTalk;
    DialogueTrigger firstIntro;
    DialogueTrigger intro; 

    PlayerController playerController;
    Animator animator;
    private FeedbackText feedbackText;
    private GameObject anai;
    private CurrentPlayer currentPlayer;


    SkyColors sky;
    bool beforeNoon = true; 

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (DialoguePanel == null) DialoguePanel = GameObject.Find("Dialogue Panel");
        sky = GameObject.Find("Sky").GetComponent<SkyColors>();

        if (GameStateController.current.Passed)
            gameObject.SetActive(false); 
        //playerController = LevelManager.current.currentPlayer.GetComponent<IPlayerController>();
        // Player = LevelManager.current.currentPlayer;
        playerController = LevelManager.current.player.GetComponent<PlayerController>();
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
        currentPlayer = LevelManager.current.player.GetComponent<CurrentPlayer>();
        anai = PlayerController.instance.AnaiObject;

        agent = GetComponent<NavMeshAgent>();

        NPCController = GetComponent<Animator>();
        Vector3 pos = transform.position; 
        npc = new NPCMovementController(gameObject, anai,Constants.PINON_NAME);
        npc.FollowPlayer(bufferDist, tooCloseRad);
        npc.Wander(WalkCenter.transform.position, wanderRad);
        npc.SetDefault(NPCMovementController.MoveState.chill);
        npc.Reset();
        npc.SetLoc(pos);


        icon = new IconFactory().GetIcon(Constants.PINON_ICON);
        firstIntro = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.PINON_FIRST_INTRO_DIALOGUE);
        firstIntro.SetExitText("Fine. I didn't want to talk to you either.");
        intro = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.PINON_INTRO_DIALOGUE);
        intro.SetExitText("You're going to leave me alone? Finally!");

        if (DataSavingManager.current.GetNPCDialogue(Constants.PINON_NAME) == null)
        {
            currConvo = Convo.first; 
            currTalk = firstIntro;
            DataSavingManager.current.SaveNPCDialogues(Constants.PINON_NAME, currTalk); 
        }
        else
        {
            currTalk = DataSavingManager.current.GetNPCDialogue(Constants.PINON_NAME);
            if(currTalk.Equals(firstIntro))
            { 
                currConvo = Convo.first;
            }
            else
            {
                currConvo = Convo.intro; 
            }
        }

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };

    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer.IsAnai())
        {
            currTalk.Update();

            npc.UpdateMovement();

            if (npc.DistanceFrom(anai) < engagementRadius && !firstIntro.Complete)
            {
                StartTalk();
                //indicateInterest();
                npc.Follow();
            }
            else if (currTalk.Complete && npc.state != NPCMovementController.MoveState.wander)
            {
                npc.SetDefault(NPCMovementController.MoveState.wander);
                npc.Wander();
                npc.Run(1.3f);
                switch(currConvo)
                {
                    case Convo.first:
                        Invoke("switchConvos", 3);
                        currConvo = Convo.intro; 
                        break;
                    case Convo.intro:
                        break; 
                }
            }
        }
        else
        {
            npc.UpdateMovement();
        }
        dialogueActive = currTalk.DialogueActive();
        NPCController.SetBool("IsTalking", dialogueActive);

        Debug.Log(sky.GetTime()); 
        if(sky.GetTime() > sky.Passout && beforeNoon)
        {
            Afternoon();
            beforeNoon = false; 
        }

    }

    public DialogueTrigger GetCurrDialogue()
    {
        return currTalk; 
    }

    private void switchConvos()
    {
        currTalk = intro;
        DataSavingManager.current.SaveNPCDialogues(Constants.PINON_NAME, currTalk);
    }

    public void Afternoon()
    {
        gameObject.SetActive(false); 
    }

    // Action Wheel Interactions
    public void Talk()
    {
        if (currTalk.Complete)
        {
            displayFeedback("Pinon doesn't want to talk to you.");
        }
        else
        {
            StartTalk();
            npc.Follow();
        }
    }
    public void Gift(string giftName)
    {
        displayFeedback("Pinon doesn't want anything from you.");
    }
    public void Distract(GameObject distractedBy)
    {
        npc.Distracted(distractedBy);
    }

    public void EndDistract()
    {
        npc.Reset();
    }


    public string Inspect()
    {
        return Constants.PINON_NAME; 
    }

    //start current conversation
    public void StartTalk()
    {
        if (!currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = gameObject;
            currTalk.StartDialogue();
        }
    }

    //end current conversation
    public void EndTalk()
    {
        //npc.Reset();

        if (currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = null;
            currTalk.EndDialogue();
        }
    }

    private void displayFeedback(string text)
    {
        feedbackText.ShowText(text);
    }

    private void indicateInterest()
    {

    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        //enabled = !isPaused;
    }



    private void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;
    }

}

