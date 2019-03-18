using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCCombatController : MonoBehaviour, ICombatController
{
    // Interface Members
    [HideInInspector] public CharacterStats Stats { get; private set; }
    [HideInInspector] public bool IsBlocking { get; private set; } = false;
    [HideInInspector] public bool InCombat { get; private set; } = false;
    [HideInInspector] public bool IsDead { get; private set; } = false;
    [HideInInspector] public bool HasWeaponOut { get; private set; } = false;
    [HideInInspector] public bool Active { get; set; } = true;

    [Header("Aggression")]
    public Aggression aggression;
    public enum Aggression { Passive, Unaggressive, Aggressive, Frenzied };
    private Coroutine deaggroCoroutine = null;
    [HideInInspector] public GameObject combatTarget = null;
    private float deaggroTime = 3;
    private List<GameObject> aggressors;

    [Header("Death Effects")]
    public GameObject ragdollPrefab;
    public GameObject deathEffect;

    private GameObject frenzyEffect;
    private float timeSinceLastHurt;
    private float hurtDelay = 0.2f;

    [Header("Weapon")]
    public Weapon weapon;
    public float attackDistance = 2.6f;
    private readonly float[] attackMultipliers = new float[] { 0f, 1f };
    public bool isAttacking;
    public bool hasRangedAttack;
    public Transform projectileNode;
    public GameObject rangedProjectile;
    public float rangedAttackCooldown = 3f;
    [HideInInspector] public int attack;
    [HideInInspector] public float attackTimeout = 20f;
    public float timeSinceLastAttack;

    private Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;
    private FieldOfView fieldOfView;
    private NPCAnimationController npcAnimationController;
    [HideInInspector] public NPCMovementController npcMovement;
    private AudioManager audioManager;
    [HideInInspector] public NPCGroup group;

    public delegate void AggroUpdate(bool aggroed, GameObject aggroTarget);
    public event AggroUpdate OnAggroUpdated;

    public delegate void DeathUpdate(NPCCombatController npc);
    public event DeathUpdate OnDeath;


    private void Awake()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
        fieldOfView = GetComponent<FieldOfView>();
        npcAnimationController = GetComponent<NPCAnimationController>();
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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
    }

    private void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;
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

        if (npcMovement != null)
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
                // Ignore this hit if group behavior disallows hurting eachother
                if (!HitAllowedByGroupBehavior(other.gameObject.transform.root.gameObject))
                    return;

                Aggro(other.gameObject.transform.root.gameObject, false);

                if (timeSinceLastHurt > hurtDelay)
                {
                    // Get hurtbox information
                    IHurtboxController hurtboxController = other.gameObject.GetComponent<IHurtboxController>();
                    GameObject source = hurtboxController.Source;
                    int damage = hurtboxController.Damage;

                    if (!IsBlocking)
                        Stagger();

                    ICombatController sourceCombatController = null;
                    sourceCombatController = source.GetComponent<NPCCombatController>();
                    if (sourceCombatController == null)
                    {
                        //sourceCombatController = source.GetComponent<PlayerCombatController>();
                        sourceCombatController = PlayerController.instance.ActivePlayerCombatControls;  // Now uses generic Player object
                    }

                    if (source)
                        Stats.TakeDamage(damage, source.name, hurtboxController.SourceCharacterStats, sourceCombatController, GetContactPoint(other), IsBlocking);
                    else
                        Stats.TakeDamage(damage, "Unknown");
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

    private bool HitAllowedByGroupBehavior(GameObject aggressor)
    {
        if (group && group.IsInGroup(aggressor))
        {
            // If we are grouped with the aggressor, only allow the hit if the group allows inter-aggression
            if (group.cantHurtEachother)
                return false;
            else
                return true;
        }
        // If we're not grouped with the aggressor, allow the hit
        else
        {
            return true;
        }
    }

    public void Stagger()
    {
        npcAnimationController.TriggerHit();
        SetStunned(1);
        //StartCoroutine(Backstep());
    }
    public void SetStunned(int stunned)
    {
        if (npcMovement != null)
        {
            if (stunned == 1)
                npcMovement.stunned = true;
            else
            {
                npcMovement.stunned = false;
                //if (Random.Range(0, 100) > 0)
                    //StartCoroutine(Backstep());
            }
        }
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
        if (Vector3.Distance(combatTarget.transform.position, transform.position) < attackDistance)
        {
            Attack();
        }
        else
        {
            if (hasRangedAttack && projectileNode && timeSinceLastAttack > rangedAttackCooldown)
                AttackProjectile();
        }
        //if (fieldOfView.IsInFieldOfView(combatTarget.transform) && Vector3.Distance(combatTarget.transform.position, transform.position) < attackDistance)
        //{
        //    Attack();
        //}

    }
    private void Attack()
    {
        isAttacking = true;
        timeSinceLastAttack = 0f;

        npcAnimationController.TriggerAttack();
    }
    private void AttackProjectile()
    {
        isAttacking = true;
        timeSinceLastAttack = 0f;

        npcAnimationController.TriggerAttackRanged();
    }
    public void LaunchProjectile()
    {
        GameObject projectile = Instantiate(rangedProjectile, projectileNode.position, Quaternion.LookRotation(combatTarget.transform.position - transform.position));
        projectile.transform.LookAt(combatTarget.transform.position);
        IProjectile proj = projectile.GetComponent<IProjectile>();
        proj.Hurtbox.SourceCharacterStats = Stats;
        proj.Hurtbox.Source = this.gameObject;
        proj.TargetTransform = combatTarget.transform;

    }

    public void AcknowledgeHaveHit(GameObject whoWeHit)
    {
        //Aggro(whoWeHit, false);
    }

    private IEnumerator Backstep()
    {
        npcMovement.Active = false;
        navMeshAgent.enabled = false;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;

        Vector3 jumpforce = (Vector3.back * 2f) + Vector3.up;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddRelativeForce(jumpforce * 6, ForceMode.Impulse);
        rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(1);

        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        navMeshAgent.enabled = true;
        npcMovement.Active = true;
    }


    private void CheckAggression()
    {
        if (InCombat)
        {
            // Deaggro if we have no combatTarget
            if (!combatTarget || !combatTarget.activeInHierarchy)
            {
                DeAggro();

                // Stop Deaggro timer
                if (deaggroCoroutine != null)
                {
                    StopCoroutine(deaggroCoroutine);
                    deaggroCoroutine = null;
                }
                return;
            }

            // Cancel Deaggro timer if we can see target
            if (fieldOfView.IsInFieldOfView(combatTarget.transform))
            {
                if (deaggroCoroutine != null)
                {
                    StopCoroutine(deaggroCoroutine);
                    deaggroCoroutine = null;
                }

            }
            // Deaggro if we cant find target in time
            else if (deaggroCoroutine == null)
                deaggroCoroutine = StartCoroutine(DeaggroTimeout());
        }

        // Search for a target OR closer target
        Transform possibleTarget = fieldOfView?.closestTarget;
        if (possibleTarget != null)
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
    public void Aggro(GameObject aggroTarget, bool forceAggression)
    {
        if (aggression > Aggression.Passive || forceAggression)
        {
            // Dont constantly aggro
            if (aggroTarget != combatTarget)
            {
                fieldOfView.SetCombatMode(true);

                if (!InCombat)
                    InCombat = true;

                combatTarget = aggroTarget;
                //Debug.Log(gameObject.name + " started combat with " + aggroTarget.name);

                // Broadcast that we've aggroed
                OnAggroUpdated?.Invoke(true, aggroTarget);
            }
        }
    }

    private void AddAggressor(GameObject aggressor)
    {
        if (!aggressors.Contains(aggressor))
            aggressors.Add(aggressor);
    }
    private void RemoveAggressor(GameObject aggressor)
    {
        if (aggressors.Contains(aggressor))
            aggressors.Remove(aggressor);
    }

    public void Subdue()
    {
        DeAggro();
        if (aggression != Aggression.Passive)
            aggression--;
    }

    private void DeAggro()
    {
        fieldOfView.SetCombatMode(false);

        if (InCombat)
            InCombat = false;

        combatTarget = null;
        //Debug.Log(gameObject.name + " stopped combat");
        Sheathe();

        if (npcMovement != null)
            npcMovement.Chill();

        // Broadcast that we've lost aggro
        OnAggroUpdated?.Invoke(false, combatTarget);
    }

    // Set aggression to Frenzied
    private void Frenzy()
    {
        aggression = Aggression.Frenzied;
        fieldOfView.targetMask |= 1 << LayerMask.NameToLayer("NPC");

        frenzyEffect = Instantiate((GameObject)Resources.Load("Effects/EnragedEffect"), transform);
        frenzyEffect.transform.position += new Vector3(0, 0.3f, 0);
    }

    // Start deaggro timer
    private IEnumerator DeaggroTimeout()
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
        OnDeath?.Invoke(this);

        // Tell the tracker we have died
        LevelManager.current.RegisterNPCDeath(gameObject);

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
        //enabled = !isPaused;
    }
}