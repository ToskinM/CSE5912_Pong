using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class LesserNPCController : MonoBehaviour
{
    public GameObject Player;
    //public GameObject WalkArea;
    public GameObject DialoguePanel;

    public float engagementRadius = 15f;
    public float tooCloseRad = 4f;
    public float bufferDist = 5f;

    private bool engaging = false;
    private NPCMovement movement;
    private NPCCombatController combatController;
    private NavMeshAgent agent;
    private AmaruDialogueTrigger talkTrig;
    private IPlayerController playerController;


    void Start()
    {
        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<NPCCombatController>();

        // Setup Movement
        //float walkRad = WalkArea.GetComponent<Renderer>().bounds.size.x;
        Vector3 walkOrigin = transform.position;
        movement = new NPCMovement(gameObject, Player, 1);
        movement.SetEngagementDistances(5, combatController.attackDistance, 1);

        //talkTrig = new AmaruDialogueTrigger(DialoguePanel, Constants.AMARU_ICON);
        playerController = Player.GetComponent<IPlayerController>();
    }

    void Update()
    {
        if (talkTrig != null)
        {
            if (playerController.Playing)
            {
                talkTrig.Update();

                if (movement.Engaging && !talkTrig.Complete)
                {
                    StartEngagement();
                }
                else if (!movement.Wandering)
                {
                    movement.ResumeWandering();
                    if (talkTrig.DialogueActive())
                    {
                        talkTrig.EndDialogue();
                    }
                }
            }
        }
        else
        {
            if (combatController.inCombat)
            {
                movement.player = combatController.combatTarget;
                movement.Attacking = true;
            }
            else
            {
                movement.Attacking = false;
            }
        }

        movement.UpdateMovement();
    }

    private void StartEngagement()
    {
        engaging = true;

        if (talkTrig != null)
            if (!talkTrig.DialogueActive())
                talkTrig.StartDialogue();
    }
}