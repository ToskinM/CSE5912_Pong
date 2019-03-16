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
    const float wanderRad = 30f;

<<<<<<< HEAD
    public bool canInspect = true;
    public bool canTalk = true;
    public bool canDistract = true;
    public bool canGift = true;
    [HideInInspector] public bool[] actionsAvailable { get; private set; }

=======
    CurrentPlayer playerInfo;
    private GameObject anai;
>>>>>>> 67bb087c59ad71985a4921cad4512b5c7df9e176
    NPCMovementController npc;
    NavMeshAgent agent;
    DialogueTrigger talkTrig;
    PlayerController playerController;
    Animator animator;
    private List<string> acceptableGifts;
    private FeedbackText feedbackText;

    // Start is called before the first frame update
    void Start()
    {
        //npc = gameObject.AddComponent<NPCMovement>();
        playerInfo = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        anai = LevelManager.current.anai;
        agent = GetComponent<NavMeshAgent>();

        npc = new NPCMovementController(gameObject, anai);
        npc.FollowPlayer(bufferDist, tooCloseRad);
        npc.Wander(WalkCenter.transform.position, wanderRad);
        npc.SetDefault(NPCMovementController.MoveState.wander);

        icon = new IconFactory().GetIcon(Constants.AMARU_ICON);

        talkTrig = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.AMARU_INTRO_DIALOGUE);
        playerController = LevelManager.current.player.GetComponent<PlayerController>();
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        acceptableGifts = new List<string>();
        acceptableGifts.Add(ItemLookup.JAR_NAME);
        acceptableGifts.Add(ItemLookup.ROPE_NAME);

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerController.Playing)
        {
            talkTrig.Update();

            npc.UpdateMovement();

            if (npc.DistanceFrom(anai) < engagementRadius && !talkTrig.Complete)
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
        //else
        //{
        //    npc.UpdateMovement();
        //}
        dialogueActive = talkTrig.DialogueActive();

    }

    // Action Wheel Interactions
    public void Talk()
    {
        StartTalk();
    }
    public void Gift(string giftName)
    {
        if(new ItemLookup().IsContainer(giftName))
        {
            displayFeedback("Amaru loves the " + giftName + "!");
        }
        else
        {
            displayFeedback("Amaru has no use for a " + giftName + "...");
        }
    }
    public void Distract()
    {

    }
    public void Inspect()
    {

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
        npc.Reset();
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

