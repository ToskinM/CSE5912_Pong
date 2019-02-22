using System.Collections;
using UnityEngine;

public class NPCCombatController : MonoBehaviour, ICombatant
{
    public enum Aggression { Passive, Unaggressive, Aggressive, Frenzied };
    public Aggression aggression;
    public bool Active { get; set; } = true;

    public GameObject ragdollPrefab;
    public Weapon weapon;
    private readonly float[] attackMultipliers = new float[] { 0f, 1f };

    public GameObject deathEffect;

    [HideInInspector] public bool IsBlocking { get; private set; }
    private bool hasWeaponOut;
    [HideInInspector] public bool inCombat; // (aggrod)
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public int attack;
    [HideInInspector] public GameObject combatTarget = null;

    //public GameObject hitIndicator;

    public CharacterStats Stats { get; private set; }

    private float timeSinceLastHurt;
    private float hurtDelay = 0.1f;
    private float deaggroTime = 3;
    private Coroutine deaggroCoroutine = null;
    public float attackDistance = 2.6f;
    [HideInInspector] public float attackTimeout = 0.5f;
    private float timeSinceLastAttack;

    private FieldOfView fieldOfView;
    private NPCAnimationController npcAnimationController;
    private NPCMovement npcMovement;

    public delegate void AggroUpdate(bool aggroed);
    public event AggroUpdate OnAggroUpdated;

    private AudioManager audioManager;

    private void Awake()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
        fieldOfView = GetComponent<FieldOfView>();
        npcAnimationController = GetComponent<NPCAnimationController>();
    }

    void Start()
    {
        //npcMovement = new NPCMovement(gameObject, GameObject.FindGameObjectWithTag("Player"), transform.position, 5, 10);
        if (!weapon)
            weapon = null;

        weapon?.gameObject.SetActive(hasWeaponOut);

        if (aggression == Aggression.Frenzied)
            Frenzy();

        StartCoroutine(GetAudioManager());
        GameStateController.OnPaused += HandlePauseEvent;
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
            isAttacking = false;
        }

        if (Active)
        {
            //npcMovement.Update();

            // Ensure weapon state is correct based on aggro
            if (inCombat && !hasWeaponOut)
                SetWeaponSheathed(false);
            else if (!inCombat && hasWeaponOut)
                SetWeaponSheathed(true);

            CheckAggression();

            // If we're in combat..
            if (inCombat)
            {
                ManageCombat();
            }
        }

        CheckDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (Active)
        {
            // Get Tag
            string tag = other.tag;

            // Handle Hurtboxes
            if (tag == "PlayerHurtbox" || tag == "Hurtbox")
            {
                Aggro(other.gameObject.transform.root.gameObject, false);

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
                        npcAnimationController.TriggerHit();

                        Stats.TakeDamage(damage, source.name);
                    }
                }
            }

            timeSinceLastHurt = 0f;
        }
    }

    public void StartFight(GameObject player)
    {
        Active = true;
        SetWeaponSheathed(false);
        inCombat = true;
        isAttacking = true;
        combatTarget = player;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(true);
    }

    // Sheathe/Unsheathe weapon
    private void SetWeaponSheathed(bool sheathed)
    {
        if (sheathed)
            Sheathe();
        else
            Unsheathe();
    }
    private void Sheathe()
    {
        if (hasWeaponOut)
        {
            weapon?.gameObject.SetActive(false);
            hasWeaponOut = false;
        }
    }
    private void Unsheathe()
    {
        if (!hasWeaponOut)
        {
            weapon?.gameObject.SetActive(true);
            hasWeaponOut = true;
        }
    }

    private void ManageCombat()
    {
        // Attack if we can see the target and they're close enough
        if (fieldOfView.IsInFieldOfView(combatTarget.transform) && Vector3.Distance(combatTarget.transform.position, transform.position) < attackDistance)
        {
            Attack();
        }

    }
    private void Attack()
    {
        isAttacking = true;
        timeSinceLastAttack = 0f;

        npcAnimationController.TriggerAttack();
    }

    private void CheckAggression()
    {
        if (inCombat)
        {
            // Deaggro if we cant find target in time
            if (combatTarget == null)
            {
                DeAggro();
                if (deaggroCoroutine != null)
                {
                    StopCoroutine(deaggroCoroutine);
                    deaggroCoroutine = null;
                }
                return;
            }

            // Deaggro if we cant find target in time
            if (fieldOfView.IsInFieldOfView(combatTarget.transform))
            {
                if (deaggroCoroutine != null)
                {
                    StopCoroutine(deaggroCoroutine);
                    deaggroCoroutine = null;
                }

            }
            else if (deaggroCoroutine == null)
                deaggroCoroutine = StartCoroutine(WaitForDeaggro());
        }

        // Search for a target OR closer target
        Transform possibleTarget = fieldOfView?.closestTarget;
        if (possibleTarget != null )
        {
            if (aggression == Aggression.Aggressive)
            {
                if (possibleTarget.tag == "Player" && combatTarget != possibleTarget)
                {
                    Aggro(possibleTarget.gameObject, false);
                }
            }
            else if (aggression == Aggression.Frenzied)
            {
                Aggro(possibleTarget.gameObject, false);
            }
        }
    }
    private void Aggro(GameObject aggroTarget, bool forceAggression)
    {
        if (aggression > Aggression.Passive || forceAggression)
        {
            //if (combatTarget != aggroTarget)
            //{
            if (!inCombat)
                inCombat = true;

            combatTarget = aggroTarget;
            //Debug.Log(gameObject.name + " started combat with " + aggroTarget.name);
            //}

            // Broadcast that we've aggroed
            OnAggroUpdated?.Invoke(true);
        }
    }

    private void DeAggro()
    {
        if (inCombat)
            inCombat = false;

        combatTarget = null;
        //Debug.Log(gameObject.name + " stopped combat");
        Sheathe();

        // Broadcast that we've lost aggro
        OnAggroUpdated?.Invoke(false);
    }

    // Set aggression to Frenzied
    private void Frenzy()
    {
        aggression = Aggression.Frenzied;
        fieldOfView.targetMask |= 1 << LayerMask.NameToLayer("NPC");
    }

    // Start deaggro timer
    private IEnumerator WaitForDeaggro()
    {
        yield return new WaitForSeconds(deaggroTime);
        DeAggro();
    }

    // Check if we should be dead
    private void CheckDeath()
    {
        if (Stats.CurrentHealth <= 0)
            StartCoroutine(Die());
    }

    // Kills this NPC
    public void Kill()
    {
        Stats.TakeDamage(Stats.CurrentHealth, "Kill Command");
    }

    // Death cleanup and Sequence
    private IEnumerator Die()
    {
        Debug.Log(gameObject.name + " has died");

        // Stop combat
        DeAggro();

        // Play and wait for death animation to finish
        npcAnimationController.SetIsDead(true);
        yield return new WaitForSeconds(1f);

        if (ragdollPrefab != null)
        {
            // Spawn ragdoll and have it match our pose
            GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            ragdoll.GetComponent<Ragdoll>().MatchPose(gameObject.GetComponentsInChildren<Transform>());
        }
        else
        {
            // Poof us away
            ObjectPoolController.current.CheckoutTemporary(deathEffect, transform, 1);
        }

        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }

    public int GetAttackDamage()
    {
        return (int)((weapon.baseDamage * (1 + (Stats.Strength * 0.25))) * attackMultipliers[1]);
    }

    // Disable player combat controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }
}