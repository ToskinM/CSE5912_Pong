using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;

public class PinonController : MonoBehaviour, INPCController
{
    private GameObject Player;
    public GameObject WalkCenter;
    public GameObject DialoguePanel;
    public Sprite icon { get; set; }
    public bool dialogueActive = false;

    const float engagementRadius = 9f;
    const float tooCloseRad = 3f;
    const float bufferDist = 4f;
    const float wanderRad = 10f;

    NPCMovementController npc;
    NavMeshAgent agent;
    DialogueTrigger talkTrig;
    IPlayerController playerController;
    Animator animator;
    private FeedbackText feedbackText;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = LevelManager.current.currentPlayer.GetComponent<IPlayerController>();
        Player = LevelManager.current.currentPlayer;
        agent = GetComponent<NavMeshAgent>();

        Vector3 pos = transform.position; 
        npc = new NPCMovementController(gameObject, Player);
        npc.FollowPlayer(bufferDist, tooCloseRad);
        npc.Wander(WalkCenter.transform.position, wanderRad);
        npc.SetDefault(NPCMovementController.MoveState.chill);
        npc.Reset();
        npc.SetLoc(pos); 

        icon = new IconFactory().GetIcon(Constants.PINON_ICON);

        talkTrig = new DialogueTrigger(DialoguePanel, icon, Constants.PINON_FIRST_INTRO_DIALOGUE);
        //playerController = Player.GetComponent<IPlayerController>();
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

    }

    // Update is called once per frame
    void Update()
    {
//        Debug.Log("wtf"); 
        //if (playerController.Playing)
        {
//            Debug.Log("Anai bud");
            talkTrig.Update();

            npc.UpdateMovement();

            if (npc.DistanceFrom(Player) < engagementRadius && !talkTrig.Complete)
            {
//                Debug.Log("Close enough!"); 
                StartTalk();
                //indicateInterest();
                npc.Follow();
            }
            else if (talkTrig.Complete)
            {
                npc.SetDefault(NPCMovementController.MoveState.wander); 
                npc.Wander();
            }
            else
            { 
                npc.Reset(); 
            }
        }
        //else
        //{
        //    Debug.Log("whyyy");
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
        displayFeedback("Pinon doesn't want anything from you.");
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
            playerController.TalkingPartner = gameObject;
            talkTrig.StartDialogue();
        }
    }

    //end current conversation
    public void EndTalk()
    {
        npc.Reset();
        if (talkTrig.DialogueActive())
        {
            playerController.TalkingPartner = null;
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

