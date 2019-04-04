﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;


public class SypaveController : MonoBehaviour, INPCController
{

    public GameObject DialoguePanel;
    public GameObject SpawnPoint; 
    public Sprite icon { get; set; }

    public float engagementRadius = 5f;
    public float tooCloseRad = 4f;
    public float bufferDist = 3.5f;
    float paceDist = 5.5f;

    public bool canInspect = true;
    public bool canTalk = true;
    public bool canDistract = true;
    public bool canGift = true;
    [HideInInspector] public bool[] actionsAvailable { get; private set; }

    CurrentPlayer playerInfo;
    private GameObject anai;
    private NPCMovementController movement;
    //private NPCCombatController combatController;
    private NavMeshAgent agent;

    enum Convo { intro, frantic, advice }
    Convo currConvo = Convo.intro;
    private DialogueTrigger currTalk;
    private DialogueTrigger intro;
    private DialogueTrigger frantic;
    private DialogueTrigger advice;

    Vector3 centerOfTown; 
    private PlayerController playerController;
    private FeedbackText feedbackText;
    SkyColors sky;
    bool beforeNoon = true; 

    void Start()
    {
        DialoguePanel = GameStateController.current.DialoguePanel; 
        //if (DialoguePanel == null) DialoguePanel = GameObject.Find("Dialogue Panel");
        sky = GameObject.Find("Sky").GetComponent<SkyColors>();
        centerOfTown = GameObject.Find("Campfire").transform.position;

        // Initialize Components
        playerInfo = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        anai = PlayerController.instance.AnaiObject; 
        agent = GetComponent<NavMeshAgent>();
       // combatController = GetComponent<NPCCombatController>();

        // Setup Movement
        Vector3 walkOrigin = transform.position;
        movement = new NPCMovementController(gameObject, anai,Constants.SYPAVE_NAME);
        movement.FollowPlayer(bufferDist, tooCloseRad);
        movement.Pace(transform.position, paceDist);
        movement.SetDefault(NPCMovementController.MoveState.pace);

        icon = new IconFactory().GetIcon(Constants.SYPAVE_ICON);
        intro = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.SYPAVE_INTRO_DIALOGUE);
        intro.SetExitText("So your 'exploration' is more important than your mother? Fine. Go.");
        frantic = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.SYPAVE_FRANTIC_DIALOGUE);
        frantic.SetExitText("You'd better be leaving to search for him!");
        advice = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.SYPAVE_ADVICE_DIALOGUE);
        advice.SetExitText("I can't believe you...");

        if (!GameStateController.current.NPCDialogues.ContainsKey(Constants.SYPAVE_NAME))
        {
            currConvo = Convo.intro; 
            currTalk = intro;
            GameStateController.current.SaveNPCDialogues(Constants.SYPAVE_NAME, currConvo.ToString(), currTalk);
        }
        else
        {
            currTalk = GameStateController.current.GetNPCDialogue(Constants.SYPAVE_NAME);
            string convo = GameStateController.current.GetNPCDiaLabel(Constants.PINON_NAME);

            if (currTalk == intro && sky.GetTime() > 12)
            {
                currTalk = frantic;
                convo = Convo.frantic.ToString();
                GameStateController.current.SaveNPCDialogues(Constants.AMARU_NAME, currConvo.ToString(), currTalk);
            }

            if (convo.Equals(Convo.intro.ToString()))
            {
                intro = currTalk; 
                currConvo = Convo.intro;
            }
            else if (convo.Equals(Convo.frantic.ToString()))
            {
                frantic = currTalk;
                Afternoon();
                currConvo = Convo.frantic;
                gameObject.transform.position = SpawnPoint.transform.position;
            }
            else
            {
                advice = currTalk; 
                currConvo = Convo.advice;
                movement.Wander(centerOfTown, 30f);
                movement.SetDefault(NPCMovementController.MoveState.wander);
                movement.InfluenceWanderSpeed(1.5f);
            }
        }


        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        playerController = PlayerController.instance.gameObject.GetComponent<PlayerController>();

//        combatController.npcMovement = movement;


        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    void Update()
    {
        float playerDist = movement.DistanceFrom(anai);  //getXZDist(transform.position, Player.transform.position);

        if (currTalk.Complete)
        {
            movement.Reset();


        }
        switch (currConvo)
        {
            case Convo.frantic:
                if (movement.state != NPCMovementController.MoveState.follow)
                    movement.Follow();

                if (movement.DistanceFrom(anai) < engagementRadius && !frantic.DialogueActive())
                {
                    StartTalk();
                    //indicateInterest();
                    movement.Follow();
                }
                if (frantic.Complete)
                {
                    //Debug.Log("switch convos");
                    currConvo = Convo.advice;
                    movement.Wander(centerOfTown, 30f);
                    movement.SetDefault(NPCMovementController.MoveState.wander);
                    movement.InfluenceWanderSpeed(1.5f);
                    Invoke("switchConvos", 3);
                    GameStateController.current.SaveNPCDialogues(Constants.PINON_NAME, currConvo.ToString(), advice);
                }
                break;
            default:
                break;
        }

        movement.UpdateMovement();

        currTalk.Update();
        if (sky.GetTime() > sky.Passout && beforeNoon)
        {
            Afternoon();
            beforeNoon = false; 
        }
    }

    private void switchConvos()
    {
        currTalk = advice;
        currConvo = Convo.advice; 
        GameStateController.current.SaveNPCDialogues(Constants.SYPAVE_NAME, currConvo.ToString(), currTalk);
    }

    public DialogueTrigger GetCurrDialogue()
    {
        return currTalk;
    }


    public void StartTalk()
    {

        movement.FollowPlayer(bufferDist);

        if (!currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = gameObject;
            currTalk.StartDialogue();
        }
        
    }
    public void EndTalk()
    {
        
        movement.Reset();

        if (currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = null;
            currTalk.EndDialogue();
        }
        
    }

    // Action Wheel Interactions
    public void Talk()
    {
        if (currTalk.Complete)
        {
            displayFeedback("Sypave told you to get inside.");
        }
        else
        {
            StartTalk();
        }
    }
    public void Gift(string giftName)
    {
        if (new ItemLookup().IsInstrument(giftName))
        {
            displayFeedback("Sypave likes the " + giftName.ToLower() + ".");
        }
        else
        {
            displayFeedback("Sypave is not interested in the " + giftName.ToLower() + ".");
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
        return Constants.SYPAVE_NAME;
    }

    public void Afternoon()
    {
        currConvo = Convo.frantic; 
        currTalk = frantic;
        GameStateController.current.SaveNPCDialogues(Constants.SYPAVE_NAME, currConvo.ToString(), currTalk);
        movement.FollowPlayer(bufferDist);
        movement.InfluenceFollowSpeed(1.5f); 

    }

    private void displayFeedback(string text)
    {
        feedbackText.ShowText(text); 
    }

    // Disable player combat controls when game is paused
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