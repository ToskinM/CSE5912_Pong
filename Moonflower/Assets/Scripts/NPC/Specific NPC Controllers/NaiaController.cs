using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events; 
using TMPro;


public class NaiaController : MonoBehaviour, INPCController
{
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

    public NPCMovementController movement { get; set; }
    private NPCCombatController combatController;
    private CharacterStats naiaStat; 
    private NavMeshAgent agent;
    private DialogueTrigger currTalk;
    private DialogueTrigger intro;
    private DialogueTrigger postFight; 
    private DialogueTrigger advice; 
    private FeedbackText feedbackText;
    Vector3 centerOfTown;

    const int playerStatBuff = 3;

    bool trainingFight = false;
    int goalHealth;
    SkyColors sky;
    bool beforeNoon = true; 

    private enum NaiaEngageType { talk, fight, chill }
    private NaiaEngageType currState = NaiaEngageType.chill;
    enum Convo { intro, postFight, advice }
    Convo currConvo; 

    void Start()
    {
        //if (DialoguePanel == null) DialoguePanel = GameObject.Find("Dialogue Panel");
        sky = GameObject.Find("Sky").GetComponent<SkyColors>();

        // Initialize Components
        agent = GetComponent<NavMeshAgent>();
        combatController = GetComponent<NPCCombatController>();
        naiaStat = GetComponent<CharacterStats>(); 

        // Setup Movement
        Vector3 walkOrigin = transform.position;
        movement = new NPCMovementController(gameObject, Constants.NAIA_NAME);
        movement.Wander(transform.position, 2);
        movement.SetDefault(NPCMovementController.MoveState.wander); 
        centerOfTown = GameObject.Find("Campfire").transform.position;

        icon = new IconFactory().GetIcon(Constants.NAIA_ICON);
        intro = new DialogueTrigger(gameObject, icon, Constants.NAIA_INTRO_DIALOGUE);
        intro.SetExitText("See you around, I guess.");
        postFight = new DialogueTrigger(gameObject, icon, Constants.NAIA_POSTFIGHT_DIALOGUE);
        postFight.SetExitText("Oh, come on. Is this because I hit too hard? I was trying to pull my punches..."); 
        advice = new DialogueTrigger(gameObject, icon, Constants.NAIA_ADVICE_DIALOGUE);
        advice.SetExitText("You can't keep running away from this.");

        if (!GameStateController.current.NPCDialogues.ContainsKey(Constants.NAIA_NAME))
        {
            currTalk = intro;
            currConvo = Convo.intro; 
            GameStateController.current.SaveNPCDialogues(Constants.NAIA_NAME, currConvo.ToString(), currTalk);
        }
        else
        {
            currTalk = GameStateController.current.GetNPCDialogue(Constants.NAIA_NAME);
            currTalk.SetSelf(gameObject);
            string convo = GameStateController.current.GetNPCDiaLabel(Constants.PINON_NAME);
            if (currTalk == intro && GameStateController.current.Passed)
            {
//                Debug.Log("naia passed"); 
                currTalk = advice;
                convo = Convo.advice.ToString(); 
                GameStateController.current.SaveNPCDialogues(Constants.NAIA_NAME, currConvo.ToString(), currTalk);
            }

            if(convo.Equals(Convo.intro.ToString()))
            {
                intro = currTalk; 
            }
            else if (convo.Equals(Convo.postFight.ToString()))
            {
                postFight = currTalk; 
            }
            else
            {
                advice = currTalk;
                Afternoon(); 
            }
        }

        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        combatController.npcMovement = movement;

        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };
    }

    void Update()
    {
        float playerDist = movement.DistanceFrom(PlayerController.instance.AnaiObject);  //getXZDist(transform.position, Player.transform.position);
        if (combatController.Active && combatController.InCombat)
        {
            currState = NaiaEngageType.fight;
            combatController.enabled = true;
        }

        switch (currState)
        {
            case NaiaEngageType.chill:
                //Debug.Log("chilling");
                combatController.Active = true;
                if (PlayerController.instance.AnaiIsActive() && PlayerController.instance.TalkingPartner == null && playerDist < engagementRadius && !currTalk.Complete)
                {
                    //StartTalk();
                }
                break;
            case NaiaEngageType.fight:
                //Debug.Log("fighting");
                combatController.Active = true;

                if (combatController.InCombat)
                {
                    movement.Follow(combatController.CombatTarget, combatController.attackDistance, 1.5f);
                    movement.SetHoldGround(true);
                }

                CharacterStats pStats = PlayerController.instance.ActivePlayerStats; 
                if (trainingFight)
                    naiaStat.Strength = pStats.Strength+3;
                else if (naiaStat.Strength < pStats.Strength * 2)
                    naiaStat.Strength = pStats.Strength * 2;

                if(trainingFight && pStats.CurrentHealth <= goalHealth)
                {
                    //Debug.Log("Let's talk"); 
                    trainingFight = false;
                    combatController.EndFight();  
                    currTalk = postFight;
                    currConvo = Convo.postFight; 
                    StartTalk(true); 
                }

                break;

            case NaiaEngageType.talk:
                //Debug.Log("talking");
                combatController.Active = false;

                if (PlayerController.instance.AnaiIsActive())
                {
                    if(movement.state != NPCMovementController.MoveState.follow)
                    {
                        movement.FollowPlayer(3.5f);
                    }

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



        movement.UpdateMovement();
        currTalk.Update();
        if (sky.GetTime() > sky.Passout && beforeNoon)
        {
            Afternoon();
            beforeNoon = false; 
        }
    }

    public DialogueTrigger GetCurrDialogue()
    {
        return currTalk;
    }


    public void Afternoon()
    {
        currTalk = advice;
        currConvo = Convo.advice; 
        GameStateController.current.SaveNPCDialogues(Constants.NAIA_NAME, currConvo.ToString(), currTalk); 
        movement.Wander(centerOfTown, 30f);
        movement.SetDefault(NPCMovementController.MoveState.wander);
        movement.InfluenceWanderSpeed(1.5f);
    }

    public void StartTalk(bool disregardCombat = false)
    {
        movement.FollowPlayer(3.5f);
        currState = NaiaEngageType.talk;
        combatController.Active = false;

        if (!currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = gameObject;
            currTalk.StartDialogue(disregardCombat);
        }
    }
    public void EndTalk()
    {
        if (currState != NaiaEngageType.fight)
        {
            movement.Reset();
        }

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
            switch (currConvo)
            {
                case Convo.intro:
                    displayFeedback("Naia's busy brooding.");
                    break;
                case Convo.advice:
                    displayFeedback("Naia's staying here to search and protect the village.");
                    break;
            }

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
        movement.Reset();
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
        trainingFight = true;
        CharacterStats pStat = PlayerController.instance.ActivePlayerStats;
        goalHealth = pStat.CurrentHealth - (int)(pStat.Strength*4.5f);
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