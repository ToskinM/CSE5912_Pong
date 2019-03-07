﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;


public class NaiaController : MonoBehaviour, INPCController
{

    public GameObject DialoguePanel;
    public GameObject EngageOptPanel;
    public Sprite Icon { get; set; }

    public float engagementRadius = 5f;
    public float tooCloseRad = 4f;
    public float bufferDist = 5f;

    CurrentPlayer playerInfo;
    private GameObject Player;
    private NPCMovementController movement;
    private NPCCombatController combatController;
    private NavMeshAgent agent;
    private DialogueTrigger talkTrig;
    private IPlayerController playerController;

    private enum NaiaEngageType { talk, fight, chill }
    private NaiaEngageType currState = NaiaEngageType.chill;
    private List<string> acceptableGifts;

    void Start()
    {
        // Initialize Components
        playerInfo = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        Player = LevelManager.current.anai.gameObject;
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<NPCCombatController>();

        // Setup Movement
        Vector3 walkOrigin = transform.position;
        movement = new NPCMovementController(gameObject, Player);
        Icon = new IconFactory().GetIcon(Constants.NAIA_ICON);
        talkTrig = new DialogueTrigger(DialoguePanel, Icon, Constants.NAIA_INTRO_DIALOGUE);


        playerController = Player.GetComponent<IPlayerController>();

        combatController.npcMovement = movement;

        acceptableGifts = new List<string>();
        acceptableGifts.Add(ItemLookup.BOW_NAME);
        acceptableGifts.Add(ItemLookup.ARROW_NAME);
    }

    void Update()
    {
        float playerDist = movement.DistanceFrom(Player);  //getXZDist(transform.position, Player.transform.position);

        switch (currState)
        {
            case NaiaEngageType.chill:
                Debug.Log("chilling");
                combatController.Active = false;
                if (playerController.Playing && playerController.TalkingPartner == null && playerDist < engagementRadius && !talkTrig.Complete)
                {
                    StartTalk();
                }
                break;
            case NaiaEngageType.fight:
                Debug.Log("fighting");
                combatController.Active = true;

                if (combatController.InCombat)
                {
                    movement.Follow(combatController.combatTarget, combatController.attackDistance, 1.5f);
                    movement.SetHoldGround(true);
                }
                break;

            case NaiaEngageType.talk:
                Debug.Log("talking");
                //combatController.Active = false;

                if (playerController.Playing)
                {

                    if (!talkTrig.Complete)
                    {
                        //talkTrig.Update();
                    }
                    else
                    {
                        EndTalk();
                        //talkTrig.EndDialogue();

                    }
                }
                break;

        }

        if (combatController.Active && combatController.InCombat)
        {
            currState = NaiaEngageType.fight;
        }

        movement.UpdateMovement();
        talkTrig.Update();
    }


    public void StartTalk()
    {
        movement.FollowPlayer(3.5f);
        currState = NaiaEngageType.talk;
        combatController.Active = false;

        if (!talkTrig.DialogueActive())
        {
            playerController.TalkingPartner = gameObject;
            talkTrig.StartDialogue();
        }
    }
    public void EndTalk()
    {
        if (currState != NaiaEngageType.fight)
            movement.Reset();

        if (talkTrig.DialogueActive())
        {
            playerController.TalkingPartner = null;
            talkTrig.EndDialogue();
        }
    }

    // Action Wheel Interactions
    public void Talk()
    {
        StartTalk();
    }
    public void Gift(string giftName)
    {
        if (acceptableGifts.Contains(giftName))
        {
            displayFeedback("Naia likes the " + giftName + ".");
        }
        else
        {
            displayFeedback("Why would Naia want the " + giftName + "?");
        }
    }
    public void Distract()
    {

    }
    public void Inspect()
    {

    }
    public void Fight()
    {
        Debug.Log("Yo time to fight");
    }

    private void displayFeedback(string text)
    {

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