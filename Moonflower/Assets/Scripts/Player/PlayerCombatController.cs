using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ICombatController
{
    // Interface Members
    [HideInInspector] public CharacterStats Stats { get; private set; }
    [HideInInspector] public bool IsBlocking { get; private set; } = false;
    [HideInInspector] public bool InCombat { get; private set; } = false;
    [HideInInspector] public bool IsDead { get; private set; } = false;
    [HideInInspector] public bool HasWeaponOut { get; private set; } = false;

    public PlayerAnimatorController animator { get; private set; }
    public PlayerMovement playerMovement;


    [HideInInspector] public bool isAttacking;
    [HideInInspector] public float attackTimeout = 0.5f;

    public bool canAttack = true;
    //public bool isBlocking;

    public GameObject ragdollPrefab;
    public Weapon weapon;
    private int damage;
    public GameObject blockPlaceholder;

    public GameObject currentPlayer;
    private GameObject anai;
    private GameObject mimbi;

    private float timeSinceLastHurt;
    private float timeSinceLastAttack;
    private readonly float hurtDelay = 0.5f;
    private readonly float[] attackMultipliers = new float[] { 1, 1.5f };

    private const float attackDelay = 0.35f;
    private const string ATTACK_AXIS = "Attack";
    private const string BLOCK_AXIS = "Block";
    private const string SHEATHE_AXIS = "Sheathe/Unsheathe";

    //private AudioManager audioManager;
    private PlayerSoundEffect playerSoundEffect;

    void Awake()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
        animator = gameObject.GetComponent<PlayerAnimatorController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        if (weapon != null)
        {
            damage = weapon.baseDamage;
            weapon.gameObject.SetActive(HasWeaponOut);
        }
        blockPlaceholder.SetActive(IsBlocking);

        GameStateController.OnPaused += HandlePauseEvent;
        GameStateController.OnFreezePlayer += HandleFreezeEvent;

        anai = GameObject.Find("Anai");
        mimbi = GameObject.Find("Mimbi");

        playerSoundEffect = GameObject.Find("Anai").GetComponent<PlayerSoundEffect>();
        currentPlayer = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetCurrentPlayer();
    }


    void Update()
    {
        UpdateCurrentPlayer();
        if (canAttack)
        {
            timeSinceLastHurt += Time.deltaTime;
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack > attackTimeout)
            {
                isAttacking = false;
            }

            // Detect attack input (on button down)
            if (Input.GetButtonDown(ATTACK_AXIS))
            {
                Attack();
            }

            // Detect sheathing input (on button down)
            if (Input.GetButtonDown(SHEATHE_AXIS))
            {
                SetWeaponSheathed(HasWeaponOut);
            }

            // Detect blocking input (on button down)
            if (Input.GetButtonDown(BLOCK_AXIS))
            {
                SetBlock(true);
            }
            else if (Input.GetButtonUp(BLOCK_AXIS))
            {
                SetBlock(false);
            }

            CheckDeath();
        }
    }

    // Sheathe/Unsheathe weapon
    public void SetWeaponSheathed(bool sheathe)
    {
        if (weapon != null)
            weapon.gameObject.SetActive(!sheathe);

        HasWeaponOut = !sheathe;
    }

    // Block
    private void SetBlock(bool blocking)
    {
        if (blocking)
        {
            IsBlocking = true;
            blockPlaceholder.SetActive(true);
        }
        else
        {
            IsBlocking = false;
            blockPlaceholder.SetActive(false);
        }
    }

    private void Attack()
    {
        if (!HasWeaponOut)
            SetWeaponSheathed(false); // take out weapon if its not already out
        else
            Swing();

    }
    private void Swing()
    {
        isAttacking = true;
        timeSinceLastAttack = 0f;

        animator.TriggerAttack();
        //Debug.Log(currentPlayer);
        bool isAnai = GameObject.Find("Anai").GetComponent<AnaiController>().Playing;
        if (currentPlayer.name == "Anai")
            playerSoundEffect.AnaiAttackSFX();
        else
            playerSoundEffect.MimbiAttackSFX();

    }

    private void OnTriggerEnter(Collider other)
    {
        // Get Tag
        string tag = other.tag;

        // Handle Hurtboxes
        if (tag == "Hurtbox")
        {
            if (timeSinceLastHurt > hurtDelay)
            {
                // Get hurtbox information
                HurtboxController hurtboxController = other.gameObject.GetComponent<HurtboxController>();
                GameObject source = hurtboxController.source;
                int damage = hurtboxController.damage;

                if (!IsBlocking)
                    Stagger();
                Stats.TakeDamage(damage, source.name, hurtboxController.sourceCharacterStats, GetContactPoint(other), IsBlocking);
            }
        }

        timeSinceLastHurt = 0f;
    }

    private Vector3 GetContactPoint(Collider other)
    {
        Vector3 locPos = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, LayerMask.GetMask("Debug")))
        {
            //Debug.Log("Point of contact: " + hit.point);
            locPos = hit.point;
        }

        return locPos;
    }

    public void Stagger()
    {
        animator.TriggerHit();
        SetStunned(1);
        if (currentPlayer = anai)
            playerSoundEffect.AnaiPunchSFX();
    }
    public void SetStunned(int stunned)
    {
        if (stunned == 1)
            playerMovement.stunned = true;
        else
            playerMovement.stunned = false;
    }

    // Check if we should be dead
    private void CheckDeath()
    {
        if (!IsDead && Stats.CurrentHealth <= 0)
        {
            IsDead = true;
            Die();
            //StartCoroutine(Die());
        }
    }
    // Kills this Character
    public void Kill()
    {
        Stats.TakeDamage(Stats.CurrentHealth, "Kill Command");
    }

    // Death cleanup and Sequence
    private void Die()
    {
        Debug.Log(gameObject.name + " has died");
        animator.TriggerDeath();
        InCombat = false;

        if (ragdollPrefab != null)
        {
            // Spawn ragdoll and have it match our pose
            GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            ragdoll.GetComponent<Ragdoll>().MatchPose(gameObject.GetComponentsInChildren<Transform>());
        }

        //gameObject.SetActive(false);

        //Destroy(gameObject, 0.5f);
    }

    public int GetAttackDamage(int attack)
    {
        if (weapon != null)
            return (int)((weapon.baseDamage * (1 + (Stats.Strength * 0.25))) * attackMultipliers[attack]);
        else
            return 0;
    }

    public void ApplyLoad()
    {
        SetWeaponSheathed(!HasWeaponOut);
        blockPlaceholder.SetActive(IsBlocking);

        damage = weapon.baseDamage;
    }


    // Disable player combat controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }

    // Disable player combat controls
    void HandleFreezeEvent(bool frozen)
    {
        enabled = !frozen;
    }

    private void UpdateCurrentPlayer()
    {
        currentPlayer = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetCurrentPlayer();
    }
}
