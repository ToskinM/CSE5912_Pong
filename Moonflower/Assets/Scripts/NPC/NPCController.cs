using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPCController : MonoBehaviour
{
    public GameObject Anai;
    public GameObject WalkArea;
    public GameObject DialoguePanel;

    const float engagementRadius = 15f;
    const float bufferRadius = 3f;
    const float tooCloseRadius = 2.5f; 
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
        npc.plane = WalkArea;
        talkTrig = new AmaruDialogueTrigger(DialoguePanel);
    }


    // Update is called once per frame
    void Update()
    {
        talkTrig.Update(); 

        float distFromPlayer = Vector3.Distance(Anai.transform.position, transform.position);
        if (distFromPlayer <= engagementRadius && !talkTrig.Complete)
        {
            npc.Wandering = false;
            startEngagement();
            if (distFromPlayer < bufferRadius)
            {
                if (distFromPlayer < tooCloseRadius)
                {
                    Vector3 targetDirection = transform.position - Anai.transform.position;
                    //Vector3 targetPosition = playerDirection.normalized * 20f + transform.position;
                    //if (agent.CalculatePath(targetPosition, new NavMeshPath()))
                    {
                        //agent.destination = targetPosition;
                        transform.Translate(targetDirection.normalized * agent.speed * Time.deltaTime);
                    }
                }
                else
                {
                    Debug.Log("chill"); 
                    npc.Chill();
                }
            }
            else
                npc.GoHere(Anai.transform.position);

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
