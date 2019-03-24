using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events; 
using TMPro;


public class NaiaController : MonoBehaviour, INPCController
{
    public static NaiaController instance; 
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

    //public UnityEvent Fight; 

    private NPCMovementController movement;
    private NPCCombatController combatController;
    private NavMeshAgent agent;
    private DialogueTrigger currTalk;
    private DialogueTrigger intro;
    private DialogueTrigger advice; 
    private FeedbackText feedbackText;
    Vector3 centerOfTown;

    int playerStatBuff = 3; 

    private enum NaiaEngageType { talk, fight, chill }
    private NaiaEngageType currState = NaiaEngageType.chill;

    void Start()
    {
        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<NPCCombatController>();

        // Setup Movement
        Vector3 walkOrigin = transform.position;
        movement = new NPCMovementController(gameObject, PlayerController.instance.AnaiObject,Constants.NAIA_NAME);
        centerOfTown = GameObject.Find("Campfire").transform.position;

        icon = new IconFactory().GetIcon(Constants.NAIA_ICON);
        intro = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.NAIA_INTRO_DIALOGUE);
        intro.SetExitText("See you around, I guess.");
        advice = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.NAIA_ADVICE_DIALOGUE);
        advice.SetExitText("You can't keep running away from this.");
        currTalk = intro; 
        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        combatController.npcMovement = movement;

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    void Update()
    {
        float playerDist = movement.DistanceFrom(PlayerController.instance.AnaiObject);  //getXZDist(transform.position, Player.transform.position);

        switch (currState)
        {
            case NaiaEngageType.chill:
                //Debug.Log("chilling");
                combatController.Active = false;
                if (PlayerController.instance.AnaiIsActive() && PlayerController.instance.TalkingPartner == null && playerDist < engagementRadius && !currTalk.Complete)
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

                if (PlayerController.instance.AnaiIsActive())
                {

                    if (currTalk.Complete)
                    {
                        movement.Reset(); 
                        EndTalk();
                        currState = NaiaEngageType.chill;
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
        currTalk.Update();
    }

    public void Afternoon()
    {
        currTalk = advice;
        movement.Wander(centerOfTown, 30f);
        movement.SetDefault(NPCMovementController.MoveState.wander);
        movement.InfluenceWanderSpeed(1.5f);
    }

    public void StartTalk()
    {
        movement.FollowPlayer(3.5f);
        currState = NaiaEngageType.talk;
        combatController.Active = false;

        if (!currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = gameObject;
            currTalk.StartDialogue();
        }
    }
    public void EndTalk()
    {
        if (currState != NaiaEngageType.fight)
            movement.Reset();

        if (currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = null;
            currTalk.EndDialogue();
        }
    }

    // Action Wheel Interactions
    public void Talk()
    {

        if (currTalk.Complete)
        {
            displayFeedback("Naia's busy brooding.");
        }
        else
        {
            StartTalk();
        }
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
    public void Distract(GameObject distractedBy)
    {
        movement.Distracted(distractedBy);
    }

    public void EndDistract()
    {

    }

    public string Inspect()
    {
        return Constants.NAIA_NAME;
    }

    public void Fight()
    {
        if (currTalk.Complete)
        {
            EndTalk();
        }
        currState = NaiaEngageType.fight;
        combatController.StartFightWithPlayer();
    }

    //If I don't do it this way, it straight up just doesn't so... yeah it sucks
    public void FightCall()
    {
        GameObject.Find("Naia").GetComponent<NaiaController>().Fight();
    }

    public void IncreasePlayerCharisma(bool pos)
    {
        if (pos)
            GameObject.Find("Player").GetComponent<CharacterStats>().Charisma += playerStatBuff;
        else
            GameObject.Find("Player").GetComponent<CharacterStats>().Charisma -= playerStatBuff;
    
    }

    public void IncreasePlayerCunning(bool pos)
    {
        if (pos)
            GameObject.Find("Player").GetComponent<CharacterStats>().Cunning += playerStatBuff;
        else
            GameObject.Find("Player").GetComponent<CharacterStats>().Cunning -= playerStatBuff;

    }

    public void GiveGiftToPlayer(string giftName)
    {
        displayFeedback("Naia gave you a " + giftName.ToLower() + "!");
        GameObject.Find("Player").GetComponent<PlayerInventory>().AddObj(giftName);
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

//[System.Serializable]
//public class FightEvent : UnityEvent<NaiaController>
//{

//}