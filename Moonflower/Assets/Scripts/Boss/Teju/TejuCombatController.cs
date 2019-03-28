using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TejuCombatController : MonoBehaviour, ICombatController
{
    public CharacterStats Stats { get; set; }
    public bool IsBlocking { get; set; }
    public bool InCombat { get; set; } = true;
    public bool IsDead { get; set; }
    public bool HasWeaponOut { get; set; }

    public GameObject combatTarget = null;
    public bool isAttacking;

    private float timeSinceLastAttack;
    private float timeSinceLastHurt;
    private float hurtDelay = 0.2f;
    public GameObject deathEffect;
    public Weapon weapon;

    public Aggression aggression;
    public enum Aggression { Passive, Unaggressive, Aggressive, Frenzied };
    private Coroutine deaggroCoroutine = null;
    private float deaggroTime = 3;

    private FieldOfView fieldOfView;
    private TejuAnimationController animationController;

    [Header("Enable/Disable Attacks")]
    public bool cryAttackEnabled;
    public bool tantrumAttackEnabled;

    [Header("Cry Attack")]
    public GameObject cryFireProjectile;
    public Transform[] eyes = new Transform[2];
    public float cryAttackCooldown = 0.3f;
    public float cryAttackBarrageCount = 3f;
    public float cryAttackBarrageDuration = 0.3f;
    public float cryAttackFireRate = 0.1f;
    public float cryAttackAimChaos = 2f;
    private Coroutine cryAttackCoroutine = null;
    public bool cryAttackReady;

    [Header("Tantrum Attack")]
    public GameObject tantrumAttackRockFallPrefab;
    public float tantrumAttackCooldown = 5f;
    public float tantrumAttackRockCount = 5f;
    public float tantrumAttackSpawnRate = 0.8f;
    public float tantrumAttackRadius = 15f;
    private Coroutine tantrumAttackCoroutine = null;
    public bool tantrumAttackReady;

    private float[] cooldowns;
    private float[] cooldownTimers;

    public delegate void AggroUpdate(bool aggroed, GameObject aggroTarget);
    public event AggroUpdate OnAggroUpdated;

    public delegate void DeathUpdate(ICombatController npc);
    public event DeathUpdate OnDeath;

    void Start()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
        fieldOfView = GetComponent<FieldOfView>();
        animationController = GetComponent<TejuAnimationController>();
        //player = PlayerController.instance.GetActivePlayerObject();

        cooldowns = new float[] { cryAttackCooldown, tantrumAttackCooldown }; // place new cooldowns here
        cooldownTimers = new float[cooldowns.Length];
    }

    void Update()
    {
        UpdateCooldowns();
        timeSinceLastHurt += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;

        // Ensure weapon state is correct based on aggro
        if (InCombat && !HasWeaponOut)
            SetWeaponSheathed(false);
        else if (!InCombat && HasWeaponOut)
            SetWeaponSheathed(true);

        //if (npcMovement != null)
        //{
        //    npcMovement.swinging = isAttacking;
        //}

        //npcMovement.Update();

        CheckAggression();

        // If we're in combat..
        if (InCombat)
        {
            if (cryAttackReady && cryAttackCoroutine == null)
            {
                cryAttackCoroutine = StartCoroutine(CryAttack());
            }
            if (tantrumAttackReady && tantrumAttackCoroutine == null)
            {
                tantrumAttackCoroutine = StartCoroutine(TantrumAttack());
            }
        }

        CheckDeath();
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < cooldowns.Length; i++)
        {
            cooldownTimers[i] += Time.deltaTime;
        }

        if (cryAttackEnabled && !cryAttackReady && cooldownTimers[0] >= cooldowns[0])
            cryAttackReady = true;

        if (tantrumAttackEnabled && !tantrumAttackReady && cooldownTimers[1] >= cooldowns[1])
            tantrumAttackReady = true;
    }

    private IEnumerator CryAttack()
    {
        animationController.TriggerAttack();
        isAttacking = true;

        for (int i = 0; i < cryAttackBarrageCount; i++)
        {
            for (int j = 0; j < eyes.Length; j++)
                LaunchProjectile(eyes[j], cryAttackAimChaos);

            yield return new WaitForSeconds(cryAttackFireRate);
        }

        timeSinceLastAttack = 0f;
        cryAttackCoroutine = null;
        cryAttackReady = false;
        cooldownTimers[0] = 0f;
        isAttacking = false;
    }
    private IEnumerator TantrumAttack()
    {
        animationController.TriggerAttack();
        isAttacking = true;

        for (int i = 0; i < tantrumAttackRockCount; i++)
        {
            Vector3 position = combatTarget.transform.position + new Vector3(Random.Range(-tantrumAttackRadius, tantrumAttackRadius), 20, Random.Range(-tantrumAttackRadius, tantrumAttackRadius));
            Instantiate(tantrumAttackRockFallPrefab, position, Quaternion.identity);

            yield return new WaitForSeconds(cryAttackFireRate);
        }

        timeSinceLastAttack = 0f;
        tantrumAttackCoroutine = null;
        tantrumAttackReady = false;
        cooldownTimers[1] = 0f;
        isAttacking = false;
    }

    public void LaunchProjectile(Transform projectileNode, float aimChaos)
    {
        GameObject projectile = Instantiate(cryFireProjectile, projectileNode.position, Quaternion.LookRotation(combatTarget.transform.position - transform.position));
        projectile.transform.LookAt(combatTarget.transform.position + new Vector3(Random.Range(-aimChaos, aimChaos), Random.Range(-aimChaos, aimChaos), Random.Range(-aimChaos, aimChaos)));
        IProjectile proj = projectile.GetComponent<IProjectile>();
        proj.Hurtbox.SourceCharacterStats = Stats;
        proj.Hurtbox.Source = this.gameObject;
        proj.TargetTransform = combatTarget.transform;
    }

    public void AcknowledgeHaveHit(GameObject whoWeHit)
    {
        throw new System.NotImplementedException();
    }

    public void Stagger()
    {
        throw new System.NotImplementedException();
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
                DeAggro();
                //deaggroCoroutine = StartCoroutine(DeaggroTimeout());
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

        // Broadcast that we've lost aggro
        OnAggroUpdated?.Invoke(false, combatTarget);
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
        animationController.SetIsDead(true);
        yield return new WaitForSeconds(1f);


        // Poof us away
        ObjectPoolController.current.CheckoutTemporary(deathEffect, transform, 1);

        gameObject.SetActive(false);

        //Destroy(gameObject, 0.5f);
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

    public int GetAttackDamage()
    {
        return (int)((weapon.baseDamage * (1 + (Stats.Strength * 0.25))));
    }
}
