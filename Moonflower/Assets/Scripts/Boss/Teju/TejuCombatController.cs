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

    public bool isAttacking;

    public float rangedAttackCooldown = 3f;
    public float timeSinceLastAttack;
    private float timeSinceLastHurt;
    private float hurtDelay = 0.2f;

    void Start()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
    }

    void Update()
    {
        timeSinceLastHurt += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;
    }

    private IEnumerator EyeFireAttack()
    {
        yield return null;
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
