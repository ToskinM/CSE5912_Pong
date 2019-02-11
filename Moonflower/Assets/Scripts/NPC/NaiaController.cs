using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 
using TMPro;

public class NaiaController : MonoBehaviour
{
    public GameObject Player;
    public GameObject DialoguePanel;
    public GameObject EngageOptPanel; 

    public float engagementRadius = 15f;
    public float tooCloseRad = 4f;
    public float bufferDist = 5f;

    private bool engaging = false;
    private NPCMovement movement;
    private NPCCombatController combatController;
    private NavMeshAgent agent;
    private NaiaDialogueTrigger talkTrig;
    private IPlayerController playerController;
    private EngagementOptionsController engageController; 

    private enum NaiaEngageType { talk, fight, chill }
    private NaiaEngageType currState = NaiaEngageType.chill;
    private bool engageMenuAllowed = true;
    private bool engageMenuWanted = true; 

    void Start()
    {
        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<NPCCombatController>();

        // Setup Movement
        Vector3 walkOrigin = transform.position;
        movement = new NPCMovement(gameObject, Player, walkOrigin, 1);
        movement.SetEngagementDistances(5, combatController.attackDistance, 1);

        talkTrig = new NaiaDialogueTrigger(DialoguePanel, Constants.NAIA_ICON);
        playerController = Player.GetComponent<IPlayerController>();

        engageController = EngageOptPanel.GetComponent<EngagementOptionsController>();

    }


    void Update()
    {
        float playerDist = getXZDist(transform.position, Player.transform.position);
        if ( engageMenuAllowed && playerDist < engagementRadius)
        {

            if(Input.GetKeyDown(KeyCode.Return))
            {
                engageMenuWanted = !engageMenuWanted; 
            }
            if (engageMenuWanted)
            {
                ShowEngagementPanel();
            }
            else
            {
                NoEngagePanel(); 
            }


        }
        else
        {
            NoEngagePanel(); 
        }

        if (currState == NaiaEngageType.chill)
        {
            if(!talkTrig.Complete)
                engageMenuAllowed = true; 
            combatController.Active = true;
        }
        else if(currState == NaiaEngageType.fight)
        {
            engageMenuAllowed = false; 
            if (combatController.inCombat)
            {
                movement.player = combatController.combatTarget;
            }
        }
        else if(currState == NaiaEngageType.talk)
        {
            engageMenuAllowed = false;
            combatController.Active = false;
            if (playerController.Playing)
            {

                if (!talkTrig.Complete)
                {
                    talkTrig.Update();
                }
                else
                {
                    movement.ResumeWandering();
                    if (talkTrig.DialogueActive())
                    {
                        talkTrig.EndDialogue();
                        currState = NaiaEngageType.fight;
                        engageMenuAllowed = false; 
                    }
                }
            }
        }

        movement.Attacking = combatController.inCombat; 
        if(combatController.Active && combatController.inCombat)
        {
            currState = NaiaEngageType.fight; 
        }

        movement.UpdateMovement();
    }


    private float getXZDist(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z);

    }

    private void ShowEngagementPanel()
    {
        engageController.EnablePanel(Constants.NAIA_ICON); 
        Button talk = engageController.GetButton(EngagementOptionsController.EngageType.talk);
        talk.onClick.AddListener(startTalking);
    }

    private void NoEngagePanel()
    {
        if(engageController.Showing)
            engageController.DisablePanel(); 
    }

    private void startTalking()
    {
        NoEngagePanel(); 
        engageMenuAllowed = false; 
        currState = NaiaEngageType.talk;
        combatController.Active = false; 
        
        talkTrig.StartDialogue();

    }
}
