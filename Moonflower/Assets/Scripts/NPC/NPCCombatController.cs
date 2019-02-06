﻿using System.Collections;
using UnityEngine;

public class NPCCombatController : MonoBehaviour, ICombatant
{
    public enum Aggression { Passive, Unaggressive, Aggressive, Frenzied };
    public Aggression aggression;

    public bool IsBlocking { get; private set; }
    public bool hasWeaponOut;
    public bool inCombat; // (aggrod)
    public bool isHit;
    //public bool isBlocking;
    public bool isAttacking;
    //public GameObject hitIndicator;

    public CharacterStats Stats { get; private set; }

    private float timeSinceLastHurt;
    private float hurtDelay = 0.5f;

    private FieldOfView fieldOfView;
    private NPCAnimationController npcAnimationController;
    private GameObject combatTarget;

    void Start()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
        fieldOfView = GetComponent<FieldOfView>();
        npcAnimationController = GetComponent<NPCAnimationController>();
    }

    void Update()
    {
        timeSinceLastHurt += Time.deltaTime;

        // Ensure weapon state is correct based on aggro
        if (inCombat && !hasWeaponOut)
            SetWeaponSheathed(false);
        else if (!inCombat && hasWeaponOut)
            SetWeaponSheathed(true);

        CheckAggression();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get Tag
        string tag = other.tag;

        // Handle Hurtboxes
        if (tag == "Hurtbox")
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

                    Debug.Log(gameObject.name + " took " + damage + " damage from " + source.name);
                    Stats.TakeDamage(damage);
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
            hasWeaponOut = false;
        }
    }
    private void Unsheathe()
    {
        if (!hasWeaponOut)
        {
            hasWeaponOut = true;
        }
    }
    private void Attack()
    {
        throw new System.NotImplementedException();
    }

    private void CheckAggression()
    {
        Transform possibleTarget = fieldOfView.closestTarget;
        if (possibleTarget != null)
        {
            switch (aggression)
            {
                case (Aggression.Aggressive):
                    {
                        if (possibleTarget.tag == "Player")
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
            if (combatTarget != aggroTarget)
            {
                if (!inCombat)
                    inCombat = true;

                combatTarget = aggroTarget;
                Debug.Log(gameObject.name + " started combat with " + aggroTarget.name);
            }
        }
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