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
    DialogueTrigger talkTrig;
    PlayerController playerController;
    Animator animator;
    private FeedbackText feedbackText;
    private GameObject anai;
    private CurrentPlayer currentPlayer; 

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //playerController = LevelManager.current.currentPlayer.GetComponent<IPlayerController>();
        // Player = LevelManager.current.currentPlayer;
        playerController = LevelManager.current.player.GetComponent<PlayerController>();
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
        currentPlayer = LevelManager.current.player.GetComponent<CurrentPlayer>();
        anai = currentPlayer.GetAnai();

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
        talkTrig = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.PINON_FIRST_INTRO_DIALOGUE);


        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer.IsAnai())
        {
            talkTrig.Update();

            npc.UpdateMovement();

            if (npc.DistanceFrom(anai) < engagementRadius && !talkTrig.Complete)
            {
                StartTalk();
                //indicateInterest();
                npc.Follow();
            }
            else if (talkTrig.Complete && npc.state != NPCMovementController.MoveState.wander)
            {
                npc.SetDefault(NPCMovementController.MoveState.wander);
                npc.Wander();
                npc.Run(1.3f);
            }
            else
            { 
                //npc.Reset(); 
            }
        }
        else
        {
            npc.UpdateMovement();
        }
        dialogueActive = talkTrig.DialogueActive();
        NPCController.SetBool("IsTalking", dialogueActive);

    }

    // Action Wheel Interactions
    public void Talk()
    {
        if (talkTrig.Complete)
        {
            displayFeedback("Pinon doesn't want to talk to you.");
        }
        else
        {
            StartTalk();
        }
    }
    public void Gift(string giftName)
    {
        displayFeedback("Pinon doesn't want anything from you.");
    }
    public void Distract()
    {

    }

    public void EndDistract()
    {

    }

    public string Inspect()
    {
        return Constants.PINON_NAME; 
    }

    //start current conversation
    public void StartTalk()
    {
        if (!talkTrig.DialogueActive())
        {
            //playerController.TalkingPartner = gameObject;
            talkTrig.StartDialogue();
        }
    }

    //end current conversation
    public void EndTalk()
    {
        //npc.Reset();

        if (talkTrig.DialogueActive())
        {
            //playerController.TalkingPartner = null;
            talkTrig.EndDialogue();
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

