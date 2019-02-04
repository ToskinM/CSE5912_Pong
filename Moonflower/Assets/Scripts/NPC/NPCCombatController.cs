using System.Collections;
using UnityEngine;

public class NPCCombatController : MonoBehaviour, ICombatant
{
    public enum Aggression { Passive, Unaggressive, Aggressive, Frenzied };
    public Aggression aggression;

    public bool IsBlocking { get; private set; }
    public bool hasWeaponOut;
    public bool inCombat;
    //public bool isBlocking;
    public bool isAttacking;
    //public GameObject hitIndicator;

    public CharacterStats Stats { get; private set; }

    private float timeSinceLastHurt;
    private float hurtDelay = 0.5f;

    void Start()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
    }

    void Update()
    {
        timeSinceLastHurt += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get Tag
        string tag = other.tag;

        // Handle Hurtboxes
        if (tag == "Hurtbox")
        {
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

    private void Attack()
    {
        throw new System.NotImplementedException();
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
    }
}
