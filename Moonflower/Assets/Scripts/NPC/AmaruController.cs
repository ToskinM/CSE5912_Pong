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
    AmaruDialogueTrigger talkTrig;

    // Start is called before the first frame update
    void Start()
    {
        //npc = gameObject.AddComponent<NPCMovement>();
        float walkRad = WalkArea.GetComponent<Renderer>().bounds.size.x;
        Vector3 walkOrigin = WalkArea.GetComponent<Renderer>().bounds.center;
        agent = GetComponent<NavMeshAgent>();

        npc = new NPCMovement(gameObject, Player, walkOrigin, walkRad, engagementRadius);
        npc.SetEngagementDistances(engagementRadius, bufferDist, tooCloseRad);

        talkTrig = new AmaruDialogueTrigger(DialoguePanel, Constants.AMARU_ICON);
    }


    // Update is called once per frame
    void Update()
    {
        talkTrig.Update();
        npc.UpdateMovement(); 

        if(npc.Engaging && !talkTrig.Complete)
        {
            startEngagement(); 
        }
        else if(!npc.Wandering)
        {
            npc.ResumeWandering();
            if (talkTrig.DialogueActive())
            {
                talkTrig.EndDialogue();
            }
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
}
