using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class OrbyJrDialogueController : MonoBehaviour, IDialogueController
{
    //public GameObject Player;
    private Sprite icon { get; set; }
    public bool DialogueActive { get; set; } = false;
    public bool Peeved { get; set; } = false; 

    public BossDoor bossDoor; 

    const float tooCloseRad = 3f;
    const float bufferDist = 4f;

    CurrentPlayer playerInfo;
    //private GameObject anai;
    NPCMovementController movement;
    NavMeshAgent agent;
    DialogueTrigger currTalk;
    DialogueTrigger talk; 
    private FeedbackText feedbackText;
    LesserNPCController mainController;

    enum Convo { talk }
    Convo currConvo;

    string charName; 

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        charName = Constants.ORBYJR_NAME;

        mainController = GetComponent<LesserNPCController>();
        movement = mainController.movement;// new NPCMovementController(gameObject, Constants.AMARU_NAME);
        movement.FollowPlayer(bufferDist, tooCloseRad);
        movement.Pace(mainController.WanderOrigin.transform.position, mainController.WanderRange); 
        movement.SetDefault(NPCMovementController.MoveState.pace);
        movement.Reset();

        icon = mainController.icon;

        talk = new DialogueTrigger(gameObject, icon, Constants.ORBYJR_BACKUP_DIALOGUE);
        talk.SetExitText("I don't like it out here...");

        if (!GameStateController.current.NPCDialogues.ContainsKey(charName))
        {
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
        if (bossDoor.Open)
            gameObject.SetActive(false); 

        if (PlayerController.instance.AnaiIsActive())
        {
            currTalk.Update();
        }
        DialogueActive = currTalk.DialogueActive();
        if (currTalk.Complete)
        {
            movement.Reset();
            mainController.inDialogue = false; 
        }
    }

    public DialogueTrigger GetCurrDialogue()
    {
        return currTalk;
    }

    // Action Wheel Interactions
    public void Talk()
    {
//        Debug.Log("to dialogue controller"); 
        if (currTalk.Complete)
        {
            displayFeedback("Junior told you to find the crystals.");
        }
        else
        {
            StartTalk();
            mainController.inDialogue = true;
        }
        movement.Follow(); 
    }

    //start current conversation
    public void StartTalk()
    {
        movement.Follow(); 
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
