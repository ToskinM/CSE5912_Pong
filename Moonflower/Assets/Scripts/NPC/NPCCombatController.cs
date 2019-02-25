using System.Collections;
using UnityEngine;

public class NPCCombatController : MonoBehaviour, ICombatController
{
    // Interface Members
    [HideInInspector] public CharacterStats Stats { get; private set; }
    [HideInInspector] public bool IsBlocking { get; private set; } = false;
    [HideInInspector] public bool InCombat { get; private set; } = false;
    [HideInInspector] public bool IsDead { get; private set; } = false;
    [HideInInspector] public bool HasWeaponOut { get; private set; } = false;

    public bool Active { get; set; } = true;

    public enum Aggression { Passive, Unaggressive, Aggressive, Frenzied };
    public Aggression aggression;

    public GameObject ragdollPrefab;
    private readonly float[] attackMultipliers = new float[] { 0f, 1f };

    public GameObject deathEffect;

    public bool isAttacking;
    public Weapon weapon;
    [HideInInspector] public int attack;
    [HideInInspector] public GameObject combatTarget = null;

    private float timeSinceLastHurt;
    private float hurtDelay = 0.2f;
    private float deaggroTime = 3;
    private Coroutine deaggroCoroutine = null;
    public float attackDistance = 2.6f;
    [HideInInspector] public float attackTimeout = 20f;
    private float timeSinceLastAttack;

    private FieldOfView fieldOfView;
    private NPCAnimationController npcAnimationController;
    public NPCMovement npcMovement;

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

        weapon?.gameObject.SetActive(HasWeaponOut);

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

        if (npcMovement)
        {
            npcMovement.swinging = isAttacking;
        }

        if (Active)
        {
            //npcMovement.Update();

            // Ensure weapon state is correct based on aggro
            if (InCombat && !HasWeaponOut)
                SetWeaponSheathed(false);
            else if (!InCombat && HasWeaponOut)
                SetWeaponSheathed(true);

            CheckAggression();

            // If we're in combat..
            if (InCombat)
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
                if (aggression == Aggression.Unaggressive)
                    aggression = Aggression.Aggressive;

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
        npcAnimationController.TriggerHit();
        SetStunned(1);
    }
    public void SetStunned(int stunned)
    {
        if (stunned == 1)
            npcMovement.stunned = true;
        else
            npcMovement.stunned = false;
    }

    public void StartFight(GameObject player)
    {
        Active = true;
        SetWeaponSheathed(false);
        InCombat = true;
        //isAttacking = true;
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
        if (HasWeaponOut)
        {
            weapon?.gameObject.SetActive(false);
            HasWeaponOut = false;
        }
    }
    private void Unsheathe()
    {
        if (!HasWeaponOut)
        {
            weapon?.gameObject.SetActive(true);
            HasWeaponOut = true;
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
        if (InCombat)
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
            if (!InCombat)
                InCombat = true;

            combatTarget = aggroTarget;
            //Debug.Log(gameObject.name + " started combat with " + aggroTarget.name);
            //}

            // Broadcast that we've aggroed
            OnAggroUpdated?.Invoke(true);
        }
    }

    private void DeAggro()
    {
        if (InCombat)
            InCombat = false;

        combatTarget = null;
        //Debug.Log(gameObject.name + " stopped combat");
        Sheathe();

        // Broadcast that we've lost aggro
        OnAggroUpdated?.Invoke(false);
        npcMovement.Chill();
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
        if (!IsDead && Stats.CurrentHealth <= 0)
        {
            IsDead = true;
            StartCoroutine(Die());
        }
    }

    // Kills this Character
    public void Kill()
    {
        Stats.TakeDamage(Stats.CurrentHealth, "Kill Command");
    }

    // Death cleanup and Sequence
    private IEnumerator Die()
    {
        //Debug.Log(gameObject.name + " has died");

        // Tell the tracker we have died
        SceneTracker.current.RegisterNPCDeath(gameObject);

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

        //Destroy(gameObject, 0.5f);
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