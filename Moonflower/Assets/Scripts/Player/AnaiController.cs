using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class AnaiController : MonoBehaviour, IPlayerController
{
    public bool active = true;
    public bool Playing { get; set; }
    public GameObject Mimbi;
    public GameObject HUD;
    public GameObject TalkingPartner { get; set; }

    Sprite icon;

    private float moveSpeed;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public float rotateSpeed = 15f;

    MimbiController mimbiController;
    PlayerCombatController playCombat;

    public PlayerMovement playMove;
    //public NPCMovement npcMove;
    public NPCMovementController npcMove;
    PlayerAnimatorController playerAnimate;
    CharacterStats stats;
    NavMeshAgent agent;
    BoxCollider boxCollider;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 2f;
    float followDist = 8f;
    SpawnPoint spawnPoint;
    public enum PlayerStates { exploring, talking, fighting, distracting }
    public PlayerStates CurrState = PlayerStates.exploring;

    private PlayerSoundEffect playerSoundEffect;

    public delegate void HitUpdate(GameObject aggressor);
    public event HitUpdate OnHit;

    void Start()
    {
        Playing = true;
        moveSpeed = 5f;
        spawnPoint = GameObject.Find("Spawner").GetComponent<SpawnPoint>();
        spawnPoint.Spawn();
        icon = new IconFactory().GetIcon(Constants.ANAI_ICON);
        Mimbi = LevelManager.current.mimbi.gameObject;
        agent = GetComponent<NavMeshAgent>();
        playMove = GetComponent<PlayerMovement>();
        playCombat = GetComponent<PlayerCombatController>();
        playerAnimate = GetComponent<PlayerAnimatorController>();
        boxCollider = GetComponent<BoxCollider>();
        stats = GetComponent<CharacterStats>();
        npcMove = new NPCMovementController(gameObject, Mimbi);
        mimbiController = Mimbi.GetComponent<MimbiController>();

        //        npcMove.Active = false;
        npcMove.FollowPlayer(followDist, tooCloseRadius);

        GameStateController.OnPaused += HandlePauseEvent;
        GameStateController.OnFreezePlayer += HandleFreezeEvent;

        //playerAnimate.playerMovement = playMove;

        LevelManager.current.anai = this;

        playerSoundEffect = GetComponent<PlayerSoundEffect>();
    }

    private void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;
        GameStateController.OnFreezePlayer += HandleFreezeEvent;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;
        GameStateController.OnFreezePlayer -= HandleFreezeEvent;
    }

    void DetectCharacterSwitchInput()
    {
        if (Input.GetButtonDown("Switch") && !LevelManager.current.currentPlayer.GetComponent<PlayerCombatController>().InCombat)
        {
            Playing = !Playing;
            Switch(Playing);
        }
    }
    public void Switch(bool switchToThis)
    {
        if (switchToThis)
        {
            playerSoundEffect.AnaiResume();
            playCombat.enabled = true;
            gameObject.layer = 10;
            tag = "Player";
            agent.enabled = false;
            //playerAnimate.playerMovement = playMove;
            boxCollider.enabled = true;

            LevelManager.current.currentPlayer = gameObject;
        }
        else
        {
            playerSoundEffect.AnaiMute();
            //            npcMove.Active = true;
            playCombat.enabled = false;
            gameObject.layer = 0;
            tag = "Companion";
            agent.enabled = true;
            //playerAnimate.playerMovement = npcMove;
            //boxCollider.enabled = false;
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
        if (active)
        {
            DetectCharacterSwitchInput();
            DetectSummonCompanionInput();

            if (Playing)
            {
                print("Anai play");
                if (CurrState == PlayerStates.talking)
                    playCombat.canAttack = false;
                else if (!playCombat.canAttack)
                    playCombat.canAttack = true;
                playMove.MovementUpdate();

            }
            else
            {
                npcMove.UpdateMovement();
            }

            if(TalkingPartner != null)
            {
                mimbiController.Chill();
            }
            else
            {
                mimbiController.Reset();
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        // Get Tag
        string tag = other.tag;

        // Handle Hurtboxes
        if (tag == "Hurtbox")
            OnHit?.Invoke(other.gameObject);
    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        //enabled = !isPaused;
    }
    // Disable player controls
    void HandleFreezeEvent(bool frozen)
    {
        playMove.Action = Actions.Chilling;
        active = !frozen;
    }
}
