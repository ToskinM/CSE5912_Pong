using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class IjapuvaDialogueController : MonoBehaviour, IDialogueController
{
    //public GameObject Player;
    private Sprite icon { get; set; }
    public bool DialogueActive { get; set; } = false;
    public bool Peeved { get; set; } = false;
    public GameObject Fire; 

    const float tooCloseRad = 3f;
    const float bufferDist = 4f;

    CurrentPlayer playerInfo;
    //private GameObject anai;
    NPCDialogueEvents diaEvent; 

    NPCMovementController movement;
    NavMeshAgent agent;
    DialogueTrigger currTalk;
    DialogueTrigger peace;
    DialogueTrigger attack;
    DialogueTrigger attackWeap;
    private FeedbackText feedbackText;

    Vector3 startLoc; 

    enum Convo { talk }
    Convo currConvo;

    string charName;


    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        startLoc = gameObject.transform.position; 
        agent = GetComponent<NavMeshAgent>();
        charName = Constants.IJAPUVA_NAME;

        diaEvent = GameStateController.current.gameObject.GetComponent<NPCDialogueEvents>(); 

        INPCController mainController = GetComponent<INPCController>();
        movement = mainController.movement;// new NPCMovementController(gameObject, Constants.AMARU_NAME);
        movement.FollowPlayer(bufferDist, tooCloseRad);
        movement.SetDefault(NPCMovementController.MoveState.chill);
        movement.Chill();

        icon = mainController.icon;

        peace = new DialogueTrigger(gameObject, icon, Constants.IJAPUVA_PEACE_DIALOGUE);
        peace.SetExitText("Too busy for stories, eh? Pity. Kids these days will never know...");
        attack = new DialogueTrigger(gameObject, icon, Constants.IJAPUVA_ATTACK_DIALOGUE);
        attack.SetExitText("You kids can't even stay for a full conversation anymore without getting bored.");
        attackWeap = new DialogueTrigger(gameObject, icon, Constants.IJAPUVA_ATTACKWEAP_DIALOGUE);
        attackWeap.SetExitText("Maybe think before you wave that stick around. Kids these days always hit first and ask questions later.");

        if (!GameStateController.current.NPCDialogues.ContainsKey(charName))
        {
            currTalk = peace;
            currConvo = Convo.talk; 
            GameStateController.current.SaveNPCDialogues(charName, currConvo.ToString(), currTalk);
        }
        else
        {
            currTalk = GameStateController.current.GetNPCDialogue(charName);
            currTalk.SetSelf(gameObject);
            currConvo = Convo.talk;
            //peace = currTalk;
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
        if(currTalk.Complete && movement.state == NPCMovementController.MoveState.follow)
        {
            movement.GoToLoc(startLoc); 
        }
        if (currTalk.Complete)
        {
            Vector3 relative = Fire.transform.position - agent.transform.position;
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * 10);

        }

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
            if(Peeved)
                displayFeedback("You have irritated Ijapuva.");
            else
                displayFeedback("Ijapuva is grinning at the fire.");
        }
        else
        {
            if (!diaEvent.WasMorePeaceful() || !diaEvent.IsNotArmed())
            {
                if (diaEvent.IsNotArmed())
                {
                    currTalk = attack;
                }
                else
                {
                    currTalk = attackWeap;
                }

            }
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

    public void Peeve()
    {
        Peeved = true; 
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
