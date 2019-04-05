﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class AmaruController : MonoBehaviour, INPCController
{
    //public GameObject Player;
    public GameObject WalkCenter;
    public GameObject DialoguePanel;
    public Sprite icon { get; set; }
    public bool dialogueActive = false;

    const float engagementRadius = 9f;
    const float tooCloseRad = 3f;
    const float bufferDist = 4f;
    const float wanderRad = 18f;

    public bool canInspect = true;
    public bool canTalk = true;
    public bool canDistract = true;
    public bool canGift = true;
    [HideInInspector] public bool[] actionsAvailable { get; private set; }

    CurrentPlayer playerInfo;
    private GameObject anai;
    NPCMovementController npc;
    NavMeshAgent agent;
    DialogueTrigger currTalk;
    DialogueTrigger intro;
    DialogueTrigger advice;
    PlayerController playerController;
    Animator animator;
    private FeedbackText feedbackText;
    Vector3 centerOfTown;
    AmaruAnimatorController amaruAnimator;

    enum Convo { intro, advice }
    Convo currConvo; 

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


        //npc = gameObject.AddComponent<NPCMovement>();
        //playerInfo = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        anai = LevelManager.current.anai;
        agent = GetComponent<NavMeshAgent>();


        npc = new NPCMovementController(gameObject, anai,Constants.AMARU_NAME);
        npc.FollowPlayer(bufferDist, tooCloseRad);
        npc.Wander(WalkCenter.transform.position, wanderRad);
        npc.SetDefault(NPCMovementController.MoveState.wander);
        centerOfTown = GameObject.Find("Campfire").transform.position;

        icon = new IconFactory().GetIcon(Constants.AMARU_ICON);

        intro = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.AMARU_INTRO_DIALOGUE);
        advice = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.AMARU_ADVICE_DIALOGUE);
        advice.SetExitText("Good luck, Anai. I hope you find him.");

        if (!GameStateController.current.NPCDialogues.ContainsKey(Constants.AMARU_NAME))
        {
            currTalk = intro;
            currConvo = Convo.intro; 
            GameStateController.current.SaveNPCDialogues(Constants.AMARU_NAME, currConvo.ToString(), currTalk);
        }
        else
        {
            currTalk = GameStateController.current.GetNPCDialogue(Constants.AMARU_NAME);
            string convo = GameStateController.current.GetNPCDiaLabel(Constants.AMARU_NAME);
            if (currTalk == intro && GameStateController.current.Passed)
            {
//                Debug.Log("amaru passed"); 
                currTalk = advice;
                convo = Convo.advice.ToString(); 
                GameStateController.current.SaveNPCDialogues(Constants.AMARU_NAME, currConvo.ToString(), currTalk);
            }

            if(convo.Equals(Convo.intro.ToString()))
            {
                intro = currTalk; 
            }
            else
            {
                advice = currTalk;
                Afternoon(); 
            }
        }


        playerController = LevelManager.current.player.GetComponent<PlayerController>();
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();


        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };

        amaruAnimator = GetComponent<AmaruAnimatorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.GetActiveCharacter() == PlayerController.PlayerCharacter.Anai)
        {
            currTalk.Update();

            npc.UpdateMovement();

            if (npc.DistanceFrom(anai) < engagementRadius && !currTalk.Complete)
            {
                //StartTalk();
                indicateInterest();
                npc.Follow();
            }
            else
            {
                npc.Reset();
            }
        }
        else
        {
            npc.UpdateMovement();
        }
        dialogueActive = currTalk.DialogueActive();


        if(sky.GetTime()>sky.Passout && beforeNoon)
        {
            Afternoon();
            beforeNoon = false; 
        }
    }

    public DialogueTrigger GetCurrDialogue()
    {
        return currTalk;
    }


    public void Afternoon()
    {
        currTalk = advice;
        currConvo = Convo.advice; 
        GameStateController.current.SaveNPCDialogues(Constants.AMARU_NAME, currConvo.ToString(), currTalk);
        npc.Wander(centerOfTown,30f);
        npc.SetDefault(NPCMovementController.MoveState.wander);
        npc.InfluenceWanderSpeed(1.5f);
    }
    // Action Wheel Interactions
    public void Talk()
    {
        if (currTalk.Complete)
        {
            displayFeedback("Amaru is busy working.");
        }
        else
        {
            StartTalk();
        }
    }
    public void Gift(string giftName)
    {
        if(new ItemLookup().IsContainer(giftName))
        {
            displayFeedback("Amaru loves the " + giftName.ToLower() + "!");
        }
        else
        {
            displayFeedback("Amaru has no use for a " + giftName.ToLower() + "...");
        }
    }
    public void Distract(GameObject distractedBy)
    {
        npc.Distracted(distractedBy);
        amaruAnimator.StartDistraction();
    }
    public void EndDistract()
    {
        amaruAnimator.EndDistraction();
        npc.Reset();
    }
    public string Inspect()
    {
        return Constants.AMARU_NAME;
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
        npc.Reset();
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
