using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class TuvichaDialogueController : MonoBehaviour, IDialogueController
{
    //public GameObject Player;
    private Sprite icon { get; set; }
    public bool DialogueActive { get; set; } = false;
    public GameObject Location; 

    const float tooCloseRad = 3f;
    const float bufferDist = 4f;

    CurrentPlayer playerInfo;
    //private GameObject anai;
    NPCMovementController movement;
    NPCAnimationController animate; 
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
        agent = GetComponent<NavMeshAgent>();
        animate = GetComponent<NPCAnimationController>(); 
        charName = Constants.TUVICHA_NAME;

        INPCController mainController = GetComponent<INPCController>();
        movement = mainController.movement;// new NPCMovementController(gameObject, Constants.AMARU_NAME);
        movement.FollowPlayer(bufferDist, tooCloseRad);
        movement.SetDefault(NPCMovementController.MoveState.chill);
        movement.Chill();

        icon = mainController.icon;

        talk = new DialogueTrigger(gameObject, icon, Constants.TUVICHA_DIALOGUE);
        talk.SetExitText("You don't need to worry about me. I'll get out of your way.");

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
        if (PlayerController.instance.AnaiIsActive())
        {
            currTalk.Update();
        }
        DialogueActive = currTalk.DialogueActive();
        if (currTalk.Complete && movement.state == NPCMovementController.MoveState.chill)
        {
//            Debug.Log("Go back to sleep"); 
            animate.sleeping = true;
         }

    }

    public void Move()
    {
     //   Debug.Log("Move"); 
        movement.Go(Location.transform.position); 
    }

    public DialogueTrigger GetCurrDialogue()
    {
        return currTalk;
    }

    // Action Wheel Interactions
    public void Talk()
    {
//        Debug.Log("start talking");
        if (currTalk.Complete)
        {
            displayFeedback("Tuvicha looks like she's going to fall back asleep.");
        }
        else
        {
            StartTalk();
        }
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
