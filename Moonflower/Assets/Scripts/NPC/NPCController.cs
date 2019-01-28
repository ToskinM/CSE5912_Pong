using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPCController : MonoBehaviour
{
    public GameObject player; 
    public GameObject plane;
    public TextMeshPro DialogueText;

    const float engagementRadius = 15f;
    const float bufferRadius = 3f;
    bool engaging = false;
    NPCMovement npc;
    NavMeshAgent agent; 

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<NPCMovement>();

        agent = GetComponent<NavMeshAgent>(); 
        npc.agent = agent;
        npc.plane = plane;
        
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distFromPlayer <= engagementRadius)
        {
            npc.Wandering = false;
            startEngagement();
            if (distFromPlayer < bufferRadius)
                npc.Chill(); 
            else
                npc.GoHere(player.transform.position);



        }
        else if(!npc.Wandering)
        {
            npc.ResumeWandering();
        }

    }

    private void startEngagement()
    {
        engaging = true;
    }
}
