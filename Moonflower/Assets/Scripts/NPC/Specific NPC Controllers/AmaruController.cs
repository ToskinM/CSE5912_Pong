using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;

public class AmaruController : MonoBehaviour, INPCController
{
    public GameObject Player;
    public GameObject WalkCenter;
    public GameObject DialoguePanel;
    public Sprite Icon { get; set; }
    public bool dialogueActive = false;

    const float engagementRadius = 5f;
    const float tooCloseRad = 4f;
    const float bufferDist = 5f;
    const float wanderRad = 30f;

    NPCMovementController npc;
    NavMeshAgent agent;
    DialogueTrigger talkTrig;
    IPlayerController playerController;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //npc = gameObject.AddComponent<NPCMovement>();
        agent = GetComponent<NavMeshAgent>();

        npc = new NPCMovementController(gameObject, Player);
        npc.FollowPlayer(bufferDist, tooCloseRad);
        npc.Wander(WalkCenter.transform.position, wanderRad);
        npc.SetDefault(NPCMovementController.MoveState.wander);

        Icon = new IconFactory().GetIcon(Constants.AMARU_ICON);

        talkTrig = new DialogueTrigger(DialoguePanel, Icon, Constants.AMARU_INTRO_DIALOGUE);
        playerController = Player.GetComponent<IPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.Playing)
        {
            talkTrig.Update();

            npc.UpdateMovement();

            if (npc.DistanceFrom(Player) < engagementRadius && playerController.TalkingPartner == null && !talkTrig.Complete)
            {
                StartTalk();
                indicateInterest();
                npc.Follow();
            }
        }
        else
        {
            npc.UpdateMovement();
        }
        dialogueActive = talkTrig.DialogueActive();

    }

    // Action Wheel Interactions
    public void Talk()
    {
        StartTalk();
    }
    public void Gift(string giftName)
    {
        Debug.Log(gameObject.name + " was given " + giftName);
    }
    public void Distract()
    {
        Debug.Log(gameObject.name + " was distracted");
    }
    public void Inspect()
    {
        Debug.Log(gameObject.name + " was inspected");
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

