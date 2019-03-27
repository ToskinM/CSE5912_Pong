using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TejuCombatController : MonoBehaviour, ICombatController
{
    public CharacterStats Stats { get; set; }
    public bool IsBlocking { get; set; }
    public bool InCombat { get; set; }
    public bool IsDead { get; set; }
    public bool HasWeaponOut { get; set; }

    public GameObject combatTarget = null;
    public bool isAttacking;

    private float timeSinceLastAttack;
    private float timeSinceLastHurt;
    private float hurtDelay = 0.2f;

    [Header("Enable/Disable Attacks")]
    public bool cryAttackEnabled;
    public bool tantrumAttackEnabled;

    [Header("Cry Attack")]
    public GameObject cryFireProjectile;
    public Transform[] eyes = new Transform[2];
    public float cryAttackCooldown = 1f;
    public float cryAttackBarrageCount = 3f;
    public float cryAttackBarrageDuration = 1f;
    public float cryAttackFireRate = 0.1f;
    public float cryAttackAimChaos = 2f;
    private Coroutine cryAttackCoroutine = null;
    public bool cryAttackReady;

    [Header("Tantrum Attack")]
    public GameObject tantrumAttackRockFallPrefab;
    public float tantrumAttackCooldown = 3f;
    public float tantrumAttackRockCount = 10f;
    public float tantrumAttackSpawnRate = 0.6f;
    public float tantrumAttackRadius = 20f;
    private Coroutine tantrumAttackCoroutine = null;
    public bool tantrumAttackReady;

    private float[] cooldowns;
    private float[] cooldownTimers;

    void Start()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
        //combatTarget = PlayerController.instance.GetActivePlayerObject();

        cooldowns = new float[] { cryAttackCooldown, tantrumAttackCooldown }; // place new cooldowns here
        cooldownTimers = new float[cooldowns.Length];
    }

    void Update()
    {
        UpdateCooldowns();
        timeSinceLastHurt += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;

        if (cryAttackReady && cryAttackCoroutine == null)
        {
            cryAttackCoroutine = StartCoroutine(CryAttack());
        }
        if (tantrumAttackReady && tantrumAttackCoroutine == null)
        {
            tantrumAttackCoroutine = StartCoroutine(TantrumAttack());
        }
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
        isAttacking = true;

        for (int i = 0; i < tantrumAttackRockCount; i++)
        {
            Instantiate(tantrumAttackRockFallPrefab, new Vector3(Random.Range(-tantrumAttackRadius, tantrumAttackRadius), 20, Random.Range(-tantrumAttackRadius, tantrumAttackRadius)), Quaternion.identity);

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
}
