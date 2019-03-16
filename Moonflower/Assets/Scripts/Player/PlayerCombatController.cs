using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ICombatController
{
    public bool active = true;

    // Interface Members
    [HideInInspector] public CharacterStats Stats { get; private set; }
    [HideInInspector] public bool IsBlocking { get; private set; } = false;
    [HideInInspector] public bool InCombat { get { return inCombat; } private set { inCombat = value; } }
    public bool inCombat;
    [HideInInspector] public bool IsDead { get; private set; } = false;
    [HideInInspector] public bool HasWeaponOut { get; private set; } = false;

    public PlayerAnimatorController animator { get; private set; }
    public PlayerMovementController playerMovement;

    private Coroutine stopCombatCoroutine = null;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public float attackTimeout = 0.5f;
    private readonly float combatTimeout = 3f;
    private readonly float combatLossDistance = 3f;
    private GameObject currentAggressor;

    public bool canAttack = true;
    //public bool isBlocking;

    public GameObject ragdollPrefab;
    public Weapon weapon;
    private int damage;
    public GameObject blockPlaceholder;

    public GameObject currentPlayer;

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

    public delegate void HitUpdate(GameObject aggressor);
    public event HitUpdate OnHit;

    void Awake()
    {
        Stats = GetComponent<CharacterStats>();
        animator = GetComponent<PlayerAnimatorController>();
        playerMovement = FindObjectOfType< PlayerMovementController >();
    }

    void Start()
    {
        if (weapon != null)
        {
            damage = weapon.baseDamage;
            weapon.gameObject.SetActive(HasWeaponOut);
        }
        blockPlaceholder.SetActive(IsBlocking);

        PlayerController.OnCharacterSwitch += SwitchActiveCharacter;
        PlayerColliderListener.OnHurtboxHit += HandleHurtboxCollision;

        playerSoundEffect = GameObject.Find("Anai").GetComponent<PlayerSoundEffect>();

        //currentPlayer = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetCurrentPlayer();
        currentPlayer = LevelManager.current.currentPlayer;

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

    void Update()
    {
        if (active)
        {
            if (currentAggressor)
                CheckAggressorDistance();

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
    }

    private void CheckAggressorDistance()
    {
        //Debug.Log(currentAggressor);

        if (Vector3.Distance(transform.position, currentAggressor.transform.position) > combatLossDistance)
        {
            // Stop combat if we get too far away
            if (stopCombatCoroutine == null)
                stopCombatCoroutine = StartCoroutine(CombatTimeout());
        }
        else
        {
            // cancel combat loss if we get to close
            if (stopCombatCoroutine != null)
            {
                StopCoroutine(stopCombatCoroutine);
                stopCombatCoroutine = null;
            }
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

    public void GoHurt()
    {
        playerMovement.body.AddForce(transform.forward * 30f, ForceMode.Impulse);
        playerMovement.body.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
    }

    // When we hit something, acknowledge it
    public void AcknowledgeHaveHit(GameObject whoWeHit)
    {
        //Debug.Log(gameObject.name + " hit " + whoWeHit.name);
        currentAggressor = whoWeHit;
        InCombat = true;
    }

    private void Swing()
    {
        isAttacking = true;
        timeSinceLastAttack = 0f;

        animator.TriggerAttack();
        //Debug.Log(currentPlayer);

        bool isAnai = gameObject == LevelManager.current.currentPlayer;
        //bool isAnai = GameObject.Find("Anai").GetComponent<AnaiController>().Playing;

        if (currentPlayer.name == "Anai")
        {}//playerSoundEffect.AnaiAttackSFX();
        else
            playerSoundEffect.MimbiAttackSFX();

    }

    void HandleHurtboxCollision(Collider other)
    {
        OnHit?.Invoke(other.gameObject);

        // Get hurtbox information
        HurtboxController hurtboxController = other.gameObject.GetComponent<HurtboxController>();
        GameObject source = hurtboxController.source;
        int damage = hurtboxController.damage;
        timeSinceLastHurt = 0f;

        if (!IsBlocking)
            Stagger();
        Stats.TakeDamage(damage, source.name, hurtboxController.sourceCharacterStats, source.GetComponent<NPCCombatController>(), GetContactPoint(other), IsBlocking);
    }

    private Vector3 GetContactPoint(Collider other)
    {
        Vector3 locPos = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(currentPlayer.transform.position, other.transform.position - currentPlayer.transform.position, out hit, LayerMask.GetMask("Debug")))
        {
            //Debug.Log("Point of contact: " + hit.point);
            locPos = hit.point;
        }

        return locPos;
    }

    private IEnumerator CombatTimeout()
    {

        yield return new WaitForSeconds(combatTimeout);
        currentAggressor = null;
        InCombat = false;
        inCombat = false;
    }

    public void Stagger()
    {
        animator.TriggerHit();
        if (LevelManager.current.currentPlayer == LevelManager.current.mimbi)
            playerSoundEffect.MimbiGetHitSFX();
        SetStunned(1);

    }
    public void SetStunned(int stunned)
    {
        if (stunned == 1)
            playerMovement.Stunned = true;
        else
            PlayerController.instance.GetComponent<PlayerMovementController>().Stunned = false;
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
            GameObject ragdoll = Instantiate(ragdollPrefab, currentPlayer.transform.position, currentPlayer.transform.rotation);
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
        //enabled = !isPaused;
    }

    // Disable player combat controls
    void HandleFreezeEvent(bool frozen)
    {
        active = !frozen;
    }

    void SwitchActiveCharacter(PlayerController.PlayerCharacter activeChar)
    {
        currentPlayer = PlayerController.instance.GetActivePlayerObject();
    }

}
