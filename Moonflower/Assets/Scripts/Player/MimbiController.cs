using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

[Serializable]
public class MimbiController : MonoBehaviour, IPlayerController
{
    public bool Playing { get; set; }
    public GameObject Anai;

    public GameObject TalkingPartner { get; set; }

    private float moveSpeed;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public float rotateSpeed = 15f;


    PlayerMovement playMove;
    PlayerAnimatorController playerAnimate;
    public NPCMovementController npcMove; 
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
        playerAnimate = GetComponent<PlayerAnimatorController>();
        boxCollider = GetComponent<BoxCollider>();
        npcMove = new NPCMovementController(gameObject, Anai);
        npcMove.WanderFollowPlayer(wanderRadius);
        npcMove.SetDefault(NPCMovementController.MoveState.wanderfollow); 

        playerAnimate.movement = npcMove;

        GameStateController.OnPaused += HandlePauseEvent;

        SceneTracker.current.mimbi = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        DetectCharacterSwitchInput();

        if (Playing)
        {
            playMove.MovementUpdate();
        }
        else
        {
            npcMove.UpdateMovement();
        }
    }

    void DetectCharacterSwitchInput()
    {
        if (Input.GetButtonDown("Switch"))
        {
            Playing = !Playing;
            Switch(Playing);
        }
    }
    public void Switch(bool switchToThis)
            {
        if (switchToThis)
        {
            playCombat.enabled = true;
                gameObject.layer = 10;
                tag = "Player";
                agent.enabled = false;
            playerAnimate.movement = playMove;
                boxCollider.enabled = true; 
            }
            else
            {
            playCombat.enabled = false;
                gameObject.layer = 0;
                tag = "Companion";
                agent.enabled = true;
            playerAnimate.movement = npcMove;
                boxCollider.enabled = false; 
            }
        }

    public void Summon()
    {
        npcMove.RunToPlayer();
    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
}
}
