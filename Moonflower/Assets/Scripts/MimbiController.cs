using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class MimbiController : MonoBehaviour
{
    public bool Playing { get; set; }
    public GameObject Anai;

    private float moveSpeed;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public float rotateSpeed = 15f;


    PlayerMovement playMove;
    PlayerAnimatorController playAnimate; 
    NPCMovement npcMove; 
    PlayerCombatController playCombat;
    NavMeshAgent agent;
    BoxCollider boxCollider; 
    const float bufferRadius = 5f;
    const float tooCloseRadius = 4f;
    float wanderRadius = 15f; 


    // Start is called before the first frame update
    void Start()
    {
        Playing = false;
        moveSpeed = 5f;

        Anai = GameObject.Find("Anai");
        agent = GetComponent<NavMeshAgent>();
        playMove = GetComponent<PlayerMovement>();
        playCombat = GetComponent<PlayerCombatController>();
        playAnimate = GetComponent<PlayerAnimatorController>();
        boxCollider = GetComponent<BoxCollider>(); 
        npcMove = new NPCMovement(gameObject, Anai, Anai.transform.position, wanderRadius);

        playAnimate.movement = npcMove;

        npcMove.SetAvoidsPlayerRadius(tooCloseRadius);  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        {
            Playing = !Playing;
            playCombat.enabled = Playing;
            if (Playing)
            {
                tag = "Player";
                //camera = Camera.main;
                agent.enabled = false;
                playAnimate.movement = playMove;
                boxCollider.enabled = true; 

            }
            else
            {
                tag = "Companion";
                agent.enabled = true;
                //enable npc movement
                playAnimate.movement = npcMove;
                boxCollider.enabled = false; 
            }
        }

        if (Playing)
        {
            playMove.MovementUpdate();
        }
        else
        {
            //Debug.Log("Mimbi is a free agent");
            npcMove.StayClose(Anai.transform.position); 
            npcMove.UpdateMovement();
        }
    }

    public void Summon()
    {
        npcMove.RunToPlayer();
    }


}
