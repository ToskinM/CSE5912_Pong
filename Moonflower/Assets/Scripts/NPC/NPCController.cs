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
    public TextMeshProUGUI DialogueText;

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
        npc.plane = WalkArea;
        talkTrig = new AmaruDialogueTrigger(DialoguePanel, DialogueText);
    }


    // Update is called once per frame
    void Update()
    {
        talkTrig.Update(); 
        float distFromPlayer = Vector3.Distance(Anai.transform.position, transform.position);
        if (distFromPlayer <= engagementRadius)
        {
            npc.Wandering = false;
            startEngagement();
            if (distFromPlayer < bufferRadius)
                npc.Chill(); 
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
