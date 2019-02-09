﻿using System.Collections;
using UnityEngine;

public class NPCCombatController : MonoBehaviour, ICombatant
{
    public enum Aggression { Passive, Unaggressive, Aggressive, Frenzied };
    public Aggression aggression;

    public bool IsBlocking { get; private set; }
    public bool hasWeaponOut;
    public GameObject weapon;
    [HideInInspector] public bool inCombat; // (aggrod)
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public int attack;
    [HideInInspector] public GameObject combatTarget;

    //public GameObject hitIndicator;

    public CharacterStats Stats { get; private set; }

    private float timeSinceLastHurt;
    private float hurtDelay = 0.5f;
    private float attackDistance = 2.6f;

    private FieldOfView fieldOfView;
    private NPCAnimationController npcAnimationController;
    private NPCMovement npcMovement;

    void Start()
    {
        //npcMovement = new NPCMovement(gameObject, GameObject.FindGameObjectWithTag("Player"), transform.position, 5, 10);
        if (!weapon)
            weapon = null;

        weapon?.SetActive(hasWeaponOut);
        Stats = gameObject.GetComponent<CharacterStats>();
        fieldOfView = GetComponent<FieldOfView>();
        npcAnimationController = GetComponent<NPCAnimationController>();
    }

    void Update()
    {
        //npcMovement.Update();
        timeSinceLastHurt += Time.deltaTime;

        // Ensure weapon state is correct based on aggro
        if (inCombat && !hasWeaponOut)
            SetWeaponSheathed(false);
        else if (!inCombat && hasWeaponOut)
            SetWeaponSheathed(true);

        CheckAggression();

        if (attack > 0)
            isAttacking = true;
        else
            isAttacking = false;

        // If we're in combat..
        if (inCombat)
        {
            ManageCombat();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get Tag
        string tag = other.tag;

        // Handle Hurtboxes
        if (tag == "PlayerHurtbox")
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
                    npcAnimationController.SetHit(1);

                    Stats.TakeDamage(damage, source.name);
                    CheckDeath();
                }
            }
        }

        timeSinceLastHurt = 0f;
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
            weapon?.SetActive(false);
            hasWeaponOut = false;
        }
    }
    private void Unsheathe()
    {
        if (!hasWeaponOut)
        {
            weapon?.SetActive(true);
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
        if (attack <= 0)
        {
            npcAnimationController.SetAttack(1);
        }
    }

    private void CheckAggression()
    {
        if (combatTarget != null && combatTarget.tag != "Player")
        {
            DeAggro();
        }

        Transform possibleTarget = fieldOfView?.closestTarget;
        if (possibleTarget != null)
        {
            switch (aggression)
            {
                case (Aggression.Aggressive):
                    {
                        if (possibleTarget.tag == "Player" && combatTarget != possibleTarget)
                        {
                            Aggro(possibleTarget.gameObject, false);
                        }
                        break;
                    }
                case (Aggression.Frenzied):
                    {
                        Aggro(possibleTarget.gameObject, false);
                        break;
                    }

                default:
                    break;
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
            Debug.Log(gameObject.name + " started combat with " + aggroTarget.name);
            //}
        }
    }
    private void DeAggro()
    {
        if (inCombat)
            inCombat = false;

        combatTarget = null;
        Debug.Log(gameObject.name + " stopped combat");
        Sheathe();
    }

    private void CheckDeath()
    {
        if (Stats.CurrentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log(gameObject.name + " has died");
    }
}