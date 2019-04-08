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
    public GameObject SpawnPoint; 
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

    public NPCMovementController movement { get; set; }
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
    //private GameObject anai;
    //private CurrentPlayer currentPlayer;


    SkyColors sky;
    bool beforeNoon = true; 

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        DialoguePanel = GameStateController.current.DialoguePanel;
        //if (DialoguePanel == null) DialoguePanel = GameObject.Find("Dialogue Panel");
        sky = GameObject.Find("Sky").GetComponent<SkyColors>();

        if (GameStateController.current.Passed)
        {
//            Debug.Log("pinon passed"); 
            gameObject.SetActive(false);
        }

        //playerController = LevelManager.current.currentPlayer.GetComponent<IPlayerController>();
        // Player = LevelManager.current.currentPlayer;
        playerController = LevelManager.current.player.GetComponent<PlayerController>();
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
        //currentPlayer = LevelManager.current.player.GetComponent<CurrentPlayer>();
        //anai = PlayerController.instance.AnaiObject;

        agent = GetComponent<NavMeshAgent>();

        NPCController = GetComponent<Animator>();
        Vector3 pos = transform.position; 
        movement = new NPCMovementController(gameObject, Constants.PINON_NAME);
        movement.FollowPlayer(bufferDist, tooCloseRad);
        movement.Wander(WalkCenter.transform.position, wanderRad);
        movement.SetDefault(NPCMovementController.MoveState.chill);
        movement.Reset();
        movement.SetLoc(pos);


        icon = new IconFactory().GetIcon(Constants.PINON_ICON);
        firstIntro = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.PINON_FIRST_INTRO_DIALOGUE);
        firstIntro.SetExitText("Fine. I didn't want to talk to you either.");
        intro = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.PINON_INTRO_DIALOGUE);
        intro.SetExitText("You're going to leave me alone? Finally!");

        if (!GameStateController.current.NPCDialogues.ContainsKey(Constants.PINON_NAME))
        {
           // Debug.Log("default");
            currConvo = Convo.first; 
            currTalk = firstIntro;
            GameStateController.current.SaveNPCDialogues(Constants.PINON_NAME, currConvo.ToString(), currTalk);

            agent.Warp(SpawnPoint.transform.position);
            //gameObject.transform.position = SpawnPoint.transform.position;
        }
        else
        {
            currTalk = GameStateController.current.GetNPCDialogue(Constants.PINON_NAME);
            currTalk.SetSelf(gameObject);
            string convo = GameStateController.current.GetNPCDiaLabel(Constants.PINON_NAME);
            if (convo.Equals(Convo.first.ToString()))
            {
                //Debug.Log("first"); 
                firstIntro = currTalk; 
                currConvo = Convo.first;
                if (currTalk.Complete)
                    currTalk = intro;
                gameObject.transform.position = SpawnPoint.transform.position;
            }
            else
            {
               // Debug.Log("intro"); 
                intro = currTalk; 
                currConvo = Convo.intro;
                movement.Wander();
            }
        }

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };



    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.AnaiIsActive())
        {
            currTalk.Update();

            movement.UpdateMovement();

            if (movement.DistanceFrom(PlayerController.instance.AnaiObject) < engagementRadius && currConvo == Convo.first && !firstIntro.Complete)
            {
                StartTalk();
                //indicateInterest();
                movement.Follow();
            }
            else if (currTalk.Complete && movement.state != NPCMovementController.MoveState.wander)
            {
                movement.SetDefault(NPCMovementController.MoveState.wander);
                movement.Wander();
                movement.Run(1.3f);
                switch(currConvo)
                {
                    case Convo.first:
                        Invoke("switchConvos", 3);
                        currConvo = Convo.intro;
                        GameStateController.current.SaveNPCDialogues(Constants.PINON_NAME, currConvo.ToString(), intro);
                        break;
                    case Convo.intro:
                        break; 
                }
            }
        }
        else
        {
            movement.UpdateMovement();
        }
        dialogueActive = currTalk.DialogueActive();
        NPCController.SetBool("IsTalking", dialogueActive);

        //Debug.Log(sky.GetTime()); 
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
        currConvo = Convo.intro; 
        GameStateController.current.SaveNPCDialogues(Constants.PINON_NAME, currConvo.ToString(), currTalk);
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
            movement.Follow();
        }
    }
    public void Gift(string giftName)
    {
        displayFeedback("Pinon doesn't want anything from you.");
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
        return Constants.PINON_NAME; 
    }

    //start current conversation
    public void StartTalk()
    {
        if (!currTalk.DialogueActive())
        {
            bool instantDialogueCam = currConvo == Convo.first ? true : false;
            //playerController.TalkingPartner = gameObject;
            currTalk.StartDialogue(false, instantDialogueCam);
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

