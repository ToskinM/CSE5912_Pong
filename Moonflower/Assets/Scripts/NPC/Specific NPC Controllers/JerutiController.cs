using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class JerutiController : MonoBehaviour, INPCController
{
    //public GameObject Player;
    public GameObject WalkCenter;
    public Sprite icon { get; set; }
    public bool dialogueActive = false;

    const float engagementRadius = 9f;
    const float tooCloseRad = 3f;
    const float bufferDist = 4f;
    const float wanderRad = 18f;

    public bool canInspect = true;
    public bool canTalk = true;
    public bool canDistract = true;
    public bool canGift = true;
    [HideInInspector] public bool[] actionsAvailable { get; private set; }

    CurrentPlayer playerInfo;
    //private GameObject anai;
    public NPCMovementController movement { get; set; }
    NavMeshAgent agent;
    DialogueTrigger currTalk;
    DialogueTrigger intro;
    DialogueTrigger introRepeat;
    DialogueTrigger advice;
    DialogueTrigger adviceRepeat;
    Animator animator;
    private FeedbackText feedbackText;
    AmaruAnimatorController amaruAnimator;

    enum Convo { intro, repIntro, advice, repAdvice}
    Convo currConvo; 

    SkyColors sky;
    bool beforeNoon = true; 

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //if (DialoguePanel == null) DialoguePanel = GameObject.Find("Dialogue Panel");
        sky = GameObject.Find("Sky").GetComponent<SkyColors>();


        //npc = gameObject.AddComponent<NPCMovement>();
        //playerInfo = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        //anai = LevelManager.current.anai;
        agent = GetComponent<NavMeshAgent>();


        movement = new NPCMovementController(gameObject, Constants.JERUTI_NAME);
        movement.FollowPlayer(bufferDist, tooCloseRad);
        movement.Reset(); 

        icon = new IconFactory().GetIcon(Constants.JERUTI_ICON);

        intro = new DialogueTrigger(gameObject, icon, Constants.JERUTI_INTRO_DIALOGUE);
        intro.SetExitText("Come back when you're ready to commit to a story.");
        introRepeat = new DialogueTrigger(gameObject, icon, Constants.JERUTI_REPINTRO_DIALOGUE);
        introRepeat.SetExitText("Come back when you're ready to commit to a story.");
        advice = new DialogueTrigger(gameObject, icon, Constants.JERUTI_ADVICE_DIALOGUE);
        intro.SetExitText("You'd better go help your brother.");
        adviceRepeat = new DialogueTrigger(gameObject, icon, Constants.JERUTI_REPADVICE_DIALOGUE);
        intro.SetExitText("You'd better go help your brother.");

        if (!GameStateController.current.NPCDialogues.ContainsKey(Constants.JERUTI_NAME))
        {
            currTalk = intro;
            currConvo = Convo.intro; 
            GameStateController.current.SaveNPCDialogues(Constants.JERUTI_NAME, currConvo.ToString(), currTalk);
        }
        else
        {
            currTalk = GameStateController.current.GetNPCDialogue(Constants.JERUTI_NAME);
            currTalk.SetSelf(gameObject);
            string convo = GameStateController.current.GetNPCDiaLabel(Constants.JERUTI_NAME);
            if ((convo.Equals(Convo.intro.ToString()) || convo.Equals(Convo.repIntro.ToString())) && GameStateController.current.Passed)
            {
//                Debug.Log("amaru passed"); 
                currTalk = advice;
                currConvo = Convo.advice;
                convo = Convo.advice.ToString();
                GameStateController.current.SaveNPCDialogues(Constants.JERUTI_NAME, currConvo.ToString(), currTalk);
            }

            if(convo.Equals(Convo.intro.ToString()))
            {
                intro = currTalk;
                currConvo = Convo.intro;
            }
            else if(convo.Equals(Convo.repIntro.ToString()))
            {
                introRepeat = currTalk;
                currConvo = Convo.repIntro;
            }
            else if (convo.Equals(Convo.advice.ToString()))
            {
                currConvo = Convo.advice;
                advice = currTalk;
            }
            else
            {
                currConvo = Convo.repAdvice;
                adviceRepeat = currTalk;
            }
        }


        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();


        actionsAvailable = new bool[] { canInspect, canTalk, canDistract, canGift };

        amaruAnimator = GetComponent<AmaruAnimatorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.AnaiIsActive())
        {
            currTalk.Update();

            movement.UpdateMovement();

            switch(currConvo)
            {
                case Convo.intro:
                    if(intro.Complete && !intro.panelInfo.IsUp)
                    {
                        currTalk = introRepeat;
                        currConvo = Convo.repIntro;
                        GameStateController.current.SaveNPCDialogues(Constants.JERUTI_NAME, currConvo.ToString(), currTalk);

                    }
                    break;
                case Convo.repIntro:
                    if (introRepeat.Complete && !intro.panelInfo.IsUp)
                    {
                        currTalk.Reset(); 
                    }
                    break;
                case Convo.advice:
                    if (advice.Complete && !intro.panelInfo.IsUp)
                    {
                        currTalk = introRepeat;
                        currConvo = Convo.repIntro;
                        GameStateController.current.SaveNPCDialogues(Constants.JERUTI_NAME, currConvo.ToString(), currTalk);

                    }
                    break;
                case Convo.repAdvice:
                    if (adviceRepeat.Complete && !intro.panelInfo.IsUp)
                    {
                        currTalk.Reset();
                    }
                    break;
            }
        }
        else
        {
            movement.UpdateMovement();
        }
        dialogueActive = currTalk.DialogueActive();


        if(sky.GetTime()>sky.Passout && beforeNoon)
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
        GameStateController.current.SaveNPCDialogues(Constants.JERUTI_NAME, currConvo.ToString(), currTalk);
    }
    // Action Wheel Interactions
    public void Talk()
    {
        if (currTalk.Complete)
        {

            switch (currConvo)
            {
                case Convo.intro:
                case Convo.repIntro:
                    displayFeedback("Jeruti is busy.");
                    break;
                case Convo.advice:
                case Convo.repAdvice: 
                    displayFeedback("Jeruti doesn't want to look.");
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
        if(new ItemLookup().IsMaterial(giftName))
        {
            displayFeedback("Jeruti appreciates the " + giftName.ToLower() + ".");
            GameStateController.current.dialogueEvents.IncreasePlayerCharisma(true);
        }
        else
        {
            displayFeedback("Jeruti looks at the " + giftName.ToLower() + " with disdain.");
        }
    }
    public void Distract(GameObject distractedBy)
    {
        movement.Distracted(distractedBy);
        amaruAnimator.StartDistraction();
    }
    public void EndDistract()
    {
        amaruAnimator.EndDistraction();
        movement.Reset();
    }
    public string Inspect()
    {
        return Constants.JERUTI_NAME;
    }

    //start current conversation
    public void StartTalk()
    {

        if (!currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = gameObject;
            currTalk.StartDialogue();
        }
    }

    //end current conversation
    public void EndTalk()
    {
        movement.Reset();
        if (currTalk.DialogueActive())
        {
            //playerController.TalkingPartner = null;
            currTalk.EndDialogue();
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
