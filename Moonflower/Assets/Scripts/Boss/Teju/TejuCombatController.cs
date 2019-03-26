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

    public Transform[] eyes = new Transform[2];
    public GameObject rangedProjectile;

    public GameObject combatTarget = null;
    public bool isAttacking;

    public float rangedAttackCooldown = 3f;
    public float timeSinceLastAttack;
    private float timeSinceLastHurt;
    private float hurtDelay = 0.2f;

    private Coroutine eyeFireAttackCoroutine = null;
    private float eyeFireAttackBarrageDuration = 1f;
    private float attackTimer;

    void Start()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
        //combatTarget = PlayerController.instance.GetActivePlayerObject();

    }

    void Update()
    {
        timeSinceLastHurt += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack < rangedAttackCooldown && eyeFireAttackCoroutine == null)
        {
            eyeFireAttackCoroutine = StartCoroutine(EyeFireAttack());
        }
    }

    private IEnumerator EyeFireAttack()
    {
        yield return null;

        attackTimer = 0f;
        while (attackTimer < eyeFireAttackBarrageDuration)
        {
            for (int i = 0; i < eyes.Length; i++)
                LaunchProjectile(eyes[i]);
            
            yield return new WaitForSeconds(0.1f);
            attackTimer += Time.deltaTime;
        }

        timeSinceLastAttack = 0f;
        eyeFireAttackCoroutine = null;
    }

    public void LaunchProjectile(Transform projectileNode)
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
        throw new System.NotImplementedException();
    }

    public void Stagger()
    {
        throw new System.NotImplementedException();
    }
}
