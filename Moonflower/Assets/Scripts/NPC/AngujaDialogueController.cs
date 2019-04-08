using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class AngujaDialogueController : MonoBehaviour, IDialogueController
{
    //public GameObject Player;
    private GameObject DialoguePanel;
    private Sprite icon { get; set; }
    public bool DialogueActive { get; set; } = false;

    const float tooCloseRad = 3f;
    const float bufferDist = 4f;

    CurrentPlayer playerInfo;
    //private GameObject anai;
    NPCMovementController movement;
    NavMeshAgent agent;
    DialogueTrigger currTalk;
    DialogueTrigger talk; 
    private FeedbackText feedbackText;

    enum Convo { talk }
    Convo currConvo;

    string charName; 

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        DialoguePanel = GameStateController.current.DialoguePanel;
        agent = GetComponent<NavMeshAgent>();
        charName = Constants.ANGUJA_NAME;

        INPCController mainController = GetComponent<INPCController>();
        movement = mainController.movement;// new NPCMovementController(gameObject, Constants.AMARU_NAME);
        movement.FollowPlayer(bufferDist, tooCloseRad);

        icon = mainController.icon;

        talk = new DialogueTrigger(gameObject, DialoguePanel, icon, Constants.ANGUJA_DIALOGUE);
        talk.SetExitText("Okay, um, nice meeting you, I guess?");

        if (!GameStateController.current.NPCDialogues.ContainsKey(charName))
        {
            Debug.Log("default"); 
            currTalk = talk;
            currConvo = Convo.talk; 
            GameStateController.current.SaveNPCDialogues(charName, currConvo.ToString(), currTalk);
        }
        else
        {
            currTalk = GameStateController.current.GetNPCDialogue(charName);
            currTalk.SetSelf(gameObject);
            currConvo = Convo.talk;
            talk = currTalk;
        }


        feedbackText = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.AnaiIsActive())
        {
            currTalk.Update();
        }
        DialogueActive = currTalk.DialogueActive();

    }

    public DialogueTrigger GetCurrDialogue()
    {
        return currTalk;
    }

    // Action Wheel Interactions
    public void Talk()
    {
        if (currTalk.Complete)
        {
            displayFeedback("Anguja is obsessing over a mild confrontation.");
        }
        else
        {
            StartTalk();
        }
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
