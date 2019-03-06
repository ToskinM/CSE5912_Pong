using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;

public class NaiaController : MonoBehaviour
{

    public GameObject DialoguePanel;
    public GameObject EngageOptPanel; 

    public float engagementRadius = 15f;
    public float tooCloseRad = 4f;
    public float bufferDist = 5f;

    CurrentPlayer playerInfo;
    private GameObject Player;
    private NPCMovementController movement;
    private NPCCombatController combatController;
    private NavMeshAgent agent;
    private NaiaDialogueTrigger talkTrig;
    private IPlayerController playerController;
    private EngagementOptionsController engageController; 

    private enum NaiaEngageType { talk, fight, chill }
    private NaiaEngageType currState = NaiaEngageType.chill;

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


        talkTrig = new NaiaDialogueTrigger(DialoguePanel, Constants.NAIA_ICON);
        playerController = Player.GetComponent<IPlayerController>();

        engageController = EngageOptPanel.GetComponent<EngagementOptionsController>();
        combatController.npcMovement = movement; 
    }

    private void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;
    }

    void Update()
    {
        float playerDist = getXZDist(transform.position, Player.transform.position);

        if (currState == NaiaEngageType.chill)
        {
            combatController.Active = true;
        }
        else if(currState == NaiaEngageType.fight)
        {
            combatController.Active = true; 

            if (combatController.InCombat)
            {
                movement.Follow(combatController.combatTarget, combatController.attackDistance, 1.5f);
                movement.SetHoldGround(true); 
            }
        }
        else if(currState == NaiaEngageType.talk)
        {
            combatController.Active = false;


            if (playerController.Playing)
            {
                if (talkTrig.engaged)
                    movement.FollowPlayer(3.5f);

                if (!talkTrig.Complete)
                {
                    talkTrig.Update();
                }
                else
                {
                    {
                        talkTrig.EndDialogue();
                        movement.Reset(); 
                        currState = NaiaEngageType.fight;
                        combatController.StartFight(Player); 
                    }
                }
            }
        }

        if(combatController.Active && combatController.InCombat)
        {
            currState = NaiaEngageType.fight; 
        }

        movement.UpdateMovement();
    }


    private float getXZDist(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z);

    }

    private void startTalking()
    {
        currState = NaiaEngageType.talk;
        combatController.Active = false;

        if (!talkTrig.DialogueActive())
        {
            talkTrig.StartDialogue();

        }

    }

    // Disable player combat controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }
}
