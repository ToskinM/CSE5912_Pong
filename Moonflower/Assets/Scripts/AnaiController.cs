using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class AnaiController : MonoBehaviour
{
    public bool Playing { get; set; }
    public GameObject Mimbi;

    private float moveSpeed;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public float rotateSpeed = 15f;


    PlayerMovement playMove;
    NPCMovement npcMove; 
    PlayerAnimatorController playAnimate; 
    PlayerCombatController playCombat; 
    NavMeshAgent agent;
    //const float hanginRadius = 15f;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 4f;
    //bool engaging = false;
    //NPCMovement npc;
    //NavMeshAgent agent;
    float wanderRadius = 8f; 


    // Start is called before the first frame update
    void Start()
    {
        Playing = true;
        moveSpeed = 5f;

        Mimbi = GameObject.Find("Mimbi"); 
        agent = GetComponent<NavMeshAgent>();
        playMove = GetComponent<PlayerMovement>();
        playCombat = GetComponent<PlayerCombatController>();
        playAnimate = GetComponent<PlayerAnimatorController>();
        npcMove = new NPCMovement(gameObject, Mimbi, Mimbi.transform.position, wanderRadius);

        playAnimate.movement = playMove; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        {
            Playing = !Playing;
            if (Playing)
            {
                playCombat.enabled = true;
                tag = "Player";
                //camera = Camera.main;
                agent.enabled = false;
                playAnimate.movement = playMove;

            }
            else
            {
                playCombat.enabled = false;
                tag = "Companion";
                agent.enabled = true;
                //enable npc movement
                playAnimate.movement = npcMove;
            }
        }

        if(Playing)
            playMove.MovementUpdate(); 
        else
        {
            npcMove.SetWanderArea(Mimbi.transform.position, wanderRadius);
            npcMove.UpdateMovement();
        }

    }
}
