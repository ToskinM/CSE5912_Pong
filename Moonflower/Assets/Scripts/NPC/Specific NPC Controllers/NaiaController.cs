using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;


public class NaiaController : MonoBehaviour, INPCController
{

    public GameObject DialoguePanel;
    public Sprite icon { get; set; }

    public float engagementRadius = 5f;
    public float tooCloseRad = 4f;
    public float bufferDist = 5f;

    public bool canInspect = true;
    public bool canTalk = true;
    public bool canDistract = true;
    public bool canGift = true;
    [HideInInspector] public bool[] actionsAvailable { get; private set; }

    CurrentPlayer playerInfo;
    private GameObject anai;
    private NPCMovementController movement;
    private NPCCombatController combatController;
    private NavMeshAgent agent;
    private DialogueTrigger talkTrig;
    private PlayerController playerController;
    private FeedbackText feedbackText; 

    private enum NaiaEngageType { talk, fight, chill }
    private NaiaEngageType currState = NaiaEngageType.chill;

    void Start()
    {
        // Initialize Components
        playerInfo = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        anai = LevelManager.current.anai;
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<NPCCombatController>();

        // Setup Movement
        Vector3 walkOrigin = transform.position;
        movement = new NPCMovementController(gameObject, anai);
        icon = new IconFactory().GetIcon(Constants.NAIA_ICON);
        talkTrig = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.NAIA_INTRO_DIALOGUE);
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        playerController = LevelManager.current.player.GetComponent<PlayerController>();

        combatController.npcMovement = movement;

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    void Update()
    {
        float playerDist = movement.DistanceFrom(anai);  //getXZDist(transform.position, Player.transform.position);

        switch (currState)
        {
            case NaiaEngageType.chill:
                //Debug.Log("chilling");
                combatController.Active = false;
                if (playerController.AnaiIsActive() && playerController.TalkingPartner == null && playerDist < engagementRadius && !talkTrig.Complete)
                {
                    //StartTalk();
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

                if (playerController.AnaiIsActive())
                {

                    if (talkTrig.Complete)
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
            //playerController.TalkingPartner = gameObject;
            talkTrig.StartDialogue();
        }
    }
    public void EndTalk()
    {
        if (currState != NaiaEngageType.fight)
            movement.Reset();

        if (talkTrig.DialogueActive())
        {
            //playerController.TalkingPartner = null;
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
        if (new ItemLookup().IsWeapon(giftName))
        {
            displayFeedback("Naia likes the " + giftName.ToLower() + ".");
        }
        else
        {
            displayFeedback("Why would Naia want the " + giftName.ToLower() + "?");
        }
    }
    public void Distract()
    {

    }
    public string Inspect()
    {
        return Constants.NAIA_NAME;
    }
    public void Fight()
    {
        Debug.Log("Yo time to fight");
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