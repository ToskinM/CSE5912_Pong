using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ICombatant
{
    public CharacterStats Stats { get; private set; }
    public PlayerAnimatorController animator { get; private set; }

    [HideInInspector] public bool IsBlocking { get; private set; } = false;
    public bool hasWeaponOut;
    [HideInInspector] public bool inCombat;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public int attack = 0;
    [HideInInspector] public float attackTimeout = 0.5f;

    //public bool isBlocking;

    public Weapon weapon;
    private int damage;
    public GameObject blockPlaceholder;

    private float timeSinceLastHurt;
    private float timeSinceLastAttack;
    private readonly float hurtDelay = 0.5f;
    private readonly float[] attackMultipliers = new float[] { 1, 1.5f };

    private const float attackDelay = 0.35f;
    private const string ATTACK_AXIS = "Attack";
    private const string BLOCK_AXIS = "Block";
    private const string SHEATHE_AXIS = "Sheathe/Unsheathe";

    private AudioManager audioManager;

    void Awake()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
        animator = gameObject.GetComponent<PlayerAnimatorController>();
    }

    void Start()
    {
        damage = weapon.baseDamage;
        weapon.gameObject.SetActive(hasWeaponOut);
        blockPlaceholder.SetActive(IsBlocking);

        StartCoroutine(GetAudioManager());
    }

    private IEnumerator GetAudioManager()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            yield return null;
        }
    }

    void Update()
    {
        timeSinceLastHurt += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > attackTimeout)
        {

        }

        // Detect attack input (on button down)
        if (Input.GetButtonDown(ATTACK_AXIS))
        {
            Attack();
        }

        // Detect sheathing input (on button down)
        if (Input.GetButtonDown(SHEATHE_AXIS))
        {
            SetWeaponSheathed(hasWeaponOut);
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
    }

    // Sheathe/Unsheathe weapon
    public void SetWeaponSheathed(bool sheathe)
    {
        weapon.gameObject.SetActive(!sheathe);

        hasWeaponOut = !sheathe;
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

    public void PlayAttackSFX()
    {
        audioManager.Play("AttackSwing");
    }

    private void Attack()
    {
        if (!hasWeaponOut)   
            SetWeaponSheathed(false); // take out weapon if its not already out
        else
            Swing();
    }
    private void Swing()
    {
        isAttacking = true;
        timeSinceLastAttack = 0f;

        animator.TriggerAttack();
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

                if (IsBlocking)
                {
                    Debug.Log(gameObject.name + ": \"Hah, blocked ya.\"");
                }
                else
                {
                    Stats.TakeDamage(damage, source.name);
                    Stagger();
                    //CheckDeath();
                }
            }
        }

        timeSinceLastHurt = 0f;
    }

    private void Stagger()
    {
        animator.TriggerHit();
    }

    public int GetAttackDamage()
    {
        return (int)((weapon.baseDamage * (1 + (Stats.Strength * 0.25))) * attackMultipliers[attack]);
    }

    public void ApplyLoad()
    {
        SetWeaponSheathed(!hasWeaponOut);
        blockPlaceholder.SetActive(IsBlocking);

        damage = weapon.baseDamage;
    }
}
