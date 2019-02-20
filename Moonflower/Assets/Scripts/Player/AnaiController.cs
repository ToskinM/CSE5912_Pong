using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class AnaiController : MonoBehaviour, IPlayerController
{
    public bool Playing { get; set; }
    public GameObject Mimbi;
    public GameObject HUD;

    Sprite icon; 

    private float moveSpeed;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public float rotateSpeed = 15f;

    MimbiController mimbiController;
    PlayerCombatController playCombat;

    public PlayerMovement playMove;
    public NPCMovement npcMove; 
    PlayerAnimatorController playAnimate; 
    CharacterStats stats; 
    NavMeshAgent agent;
    BoxCollider boxCollider;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 2f;
    float followDist = 8f; 


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
        stats = GetComponent<CharacterStats>(); 
        npcMove = new NPCMovement(gameObject, Mimbi);

        npcMove.SetFollowingDist(followDist); 
        npcMove.SetAvoidsPlayerRadius(tooCloseRadius);

        playAnimate.movement = playMove;
    }

    void DetectCharacterSwitchInput()
    {
        if (Input.GetButtonDown("Switch"))
        {
            Playing = !Playing;
            if (Playing)
            {
                playCombat.enabled = true;
                gameObject.layer = 10;
                tag = "Player";
                //camera = Camera.main;
                agent.enabled = false;
                playAnimate.movement = playMove;
                boxCollider.enabled = true;

            }
            else
            {
                playCombat.enabled = false;
                gameObject.layer = 0;
                tag = "Companion";
                agent.enabled = true;
                playAnimate.movement = npcMove;
                boxCollider.enabled = false;
            }
        }
    }

    void DetectSummonCompanionInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Mimbi.GetComponent<MimbiController>().Summon();
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectCharacterSwitchInput();
        DetectSummonCompanionInput();

        if (Playing)
        {
            playMove.MovementUpdate(); 
        }
        else
        {
            npcMove.UpdateMovement();
        }

    }

}
