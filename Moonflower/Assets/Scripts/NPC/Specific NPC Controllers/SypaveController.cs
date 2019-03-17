using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;


public class SypaveController : MonoBehaviour, INPCController
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
        icon = new IconFactory().GetIcon(Constants.SYPAVE_ICON);
        //talkTrig = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.NAIA_INTRO_DIALOGUE);
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        playerController = LevelManager.current.player.GetComponent<PlayerController>();

        combatController.npcMovement = movement;


        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    void Update()
    {
        float playerDist = movement.DistanceFrom(anai);  //getXZDist(transform.position, Player.transform.position);

        movement.UpdateMovement();

        if(talkTrig != null)
            talkTrig.Update();
    }


    public void StartTalk()
    {
        if (talkTrig != null)
        {
            movement.FollowPlayer(3.5f);
            combatController.Active = false;

            if (!talkTrig.DialogueActive())
            {
                //playerController.TalkingPartner = gameObject;
                talkTrig.StartDialogue();
            }
        }
    }
    public void EndTalk()
    {
        if (talkTrig != null)
        {
            movement.Reset();

            if (talkTrig.DialogueActive())
            {
                //playerController.TalkingPartner = null;
                talkTrig.EndDialogue();
            }
        }
    }

    // Action Wheel Interactions
    public void Talk()
    {
        StartTalk();
    }
    public void Gift(string giftName)
    {
        if (new ItemLookup().IsInstrument(giftName))
        {
            displayFeedback("Sypave likes the " + giftName + ".");
        }
        else
        {
            displayFeedback("Sypave is not interested in the " + giftName + "?");
        }
    }
    public void Distract()
    {

    }
    public string Inspect()
    {
        return Constants.SYPAVE_NAME;
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