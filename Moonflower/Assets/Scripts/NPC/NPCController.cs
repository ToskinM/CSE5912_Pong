using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPCController : MonoBehaviour
{
    public GameObject player; 
    public GameObject plane;
    //public TextMeshPro DialogueText;

    const float engagementRadius = 15f;
    const float bufferRadius = 3f;
    bool engaging = false;
    NPCMovement npc;
    NavMeshAgent agent;
    AmaruDialogueTrigger talkTrig; 

    // Start is called before the first frame update
    void Start()
    {
        npc = gameObject.AddComponent<NPCMovement>();

        agent = GetComponent<NavMeshAgent>(); 
        npc.agent = agent;
        npc.plane = plane;
        talkTrig = new AmaruDialogueTrigger(); 
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("looking...");
        float distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distFromPlayer <= engagementRadius)
        {
            Debug.Log("She's here!");
            if(!talkTrig.DialogueActive())
                talkTrig.StartDialogue();

            npc.Wandering = false;
            startEngagement();
            if (distFromPlayer < bufferRadius)
            {
                npc.Chill();
            }
            else
            { 
                npc.GoHere(player.transform.position);
            }



        }
        else if(!npc.Wandering)
        {
            npc.ResumeWandering();
            talkTrig.EndDialogue();
        }

    }

    private void startEngagement()
    {
        engaging = true;
    }
}
