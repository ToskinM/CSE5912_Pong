using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AmaruController : MonoBehaviour
{
    public GameObject Player;
    public GameObject WalkArea;
    public GameObject DialoguePanel;

    const float engagementRadius = 15f;
    const float tooCloseRad = 4f;
    const float bufferDist = 5f; 
    bool engaging = false;
    NPCMovement npc;
    NavMeshAgent agent;
    DialogueTrigger talkTrig;
    IPlayerController playerController; 
    

    // Start is called before the first frame update
    void Start()
    {
        //npc = gameObject.AddComponent<NPCMovement>();
        float walkRad = WalkArea.GetComponent<Renderer>().bounds.size.x;
        Vector3 walkOrigin = WalkArea.GetComponent<Renderer>().bounds.center;
        agent = GetComponent<NavMeshAgent>();

        npc = new NPCMovement(gameObject, Player, walkOrigin, walkRad, engagementRadius);
        npc.SetEngagementDistances(engagementRadius, bufferDist, tooCloseRad);

        talkTrig = new DialogueTrigger(DialoguePanel, Constants.AMARU_ICON, Constants.AMARU_INTRO_DIALOGUE);
        playerController = Player.GetComponent<IPlayerController>();

        GameStateController.OnPaused += HandlePauseEvent;
    }


    // Update is called once per frame
    void Update()
    {
        if (playerController.Playing)
        {
            talkTrig.Update();

            npc.UpdateMovement();

            if (npc.Engaging && !talkTrig.Complete)
            {
                startEngagement();
            }
            else if (!npc.Wandering)
            {
                npc.ResumeWandering();
                if (talkTrig.DialogueActive())
                {
                    talkTrig.EndDialogue();
                }
            }
        }
        else
        {
            npc.UpdateMovement();
        }

    }

    private void startEngagement()
    {
        engaging = true;

        if (!talkTrig.DialogueActive())
        {
            talkTrig.StartDialogue();
        }
    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }
}
