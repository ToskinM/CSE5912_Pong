using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AmaruController : MonoBehaviour
{
    public GameObject Player;
    public GameObject WalkCenter;
    public GameObject DialoguePanel;
    public bool dialogueActive = false;

    const float engagementRadius = 15f;
    const float tooCloseRad = 4f;
    const float bufferDist = 5f;
    const float wanderRad = 30f; 
    bool engaging = false;
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

        npc = new NPCMovementController(gameObject,Player);
        npc.FollowPlayer(bufferDist, tooCloseRad); 
        npc.Wander(WalkCenter.transform.position, wanderRad);
        npc.SetDefault(NPCMovementController.MoveState.wander); 

        talkTrig = new DialogueTrigger(DialoguePanel, Constants.AMARU_ICON, Constants.AMARU_INTRO_DIALOGUE);
        playerController = Player.GetComponent<IPlayerController>();

        GameStateController.OnPaused += HandlePauseEvent;
    }


    // Update is called once per frame
    void Update()
    {
        if (playerController.Playing)
        {
            //if(talkTrig.engaged && playerController.TalkingPartner!=gameObject)
            //    playerController.TalkingPartner = gameObject;
            //else if(!talkTrig.engaged && playerController.TalkingPartner!=null)
            //    playerController.TalkingPartner = null;

            talkTrig.Update();

            npc.UpdateMovement();

            if (npc.DistanceFrom(Player) < engagementRadius && !talkTrig.Complete)
            {
                startEngagement(); 
                npc.Follow(); 
            }
            else if (npc.state != NPCMovementController.MoveState.wander)
            {
                npc.Wander();
                if (talkTrig.DialogueActive())
                {
                    playerController.TalkingPartner = null;
                    talkTrig.EndDialogue();
                }
            }
        }
        else
        {
            npc.UpdateMovement();
        }
        dialogueActive = talkTrig.DialogueActive();

    }

    private void startEngagement()
    {
        playerController.TalkingPartner = gameObject;

        engaging = true;

        if (!talkTrig.DialogueActive())
        {
            talkTrig.StartDialogue();
        }
    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }


}
