using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class AnaiController : MonoBehaviour
{
    public bool Playing { get; set; }
    public GameObject Mimbi;

    Sprite icon; 

    private float moveSpeed;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public float rotateSpeed = 15f;

    MimbiController mimbiController; 

    PlayerMovement playMove;
    NPCMovement npcMove; 
    PlayerAnimatorController playAnimate; 
    PlayerCombatController playCombat; 
    NavMeshAgent agent;
    BoxCollider boxCollider;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 2f;
    float followDist = 8f; 


    // Start is called before the first frame update
    void Start()
    {
        Playing = true;
        moveSpeed = 5f;

        icon = new IconFactory().GetIcon(Constants.ANAI_ICON); 
        Mimbi = GameObject.Find("Mimbi");
        agent = GetComponent<NavMeshAgent>();
        playMove = GetComponent<PlayerMovement>();
        playCombat = GetComponent<PlayerCombatController>();
        playAnimate = GetComponent<PlayerAnimatorController>();
        boxCollider = GetComponent<BoxCollider>(); 
        npcMove = new NPCMovement(gameObject, Mimbi);

        npcMove.SetFollowingDist(followDist); 
        npcMove.SetAvoidsPlayerRadius(tooCloseRadius);

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
                boxCollider.enabled = true; 

            }
            else
            {
                playCombat.enabled = false;
                tag = "Companion";
                agent.enabled = true;
                playAnimate.movement = npcMove;
                boxCollider.enabled = false; 
            }
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            Mimbi.GetComponent<MimbiController>().Summon(); 
        }

        if (Playing)
            playMove.MovementUpdate(); 
        else
        {
            npcMove.UpdateMovement();
        }

    }

    
}
