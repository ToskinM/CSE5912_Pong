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

    public CharacterStats Stats { get; private set; }

    void Start()
    {
        Stats = gameObject.GetComponent<CharacterStats>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter. Tag = " + other.tag);
        if (other.tag == "WeaponHitbox")
        {
            //Stats.TakeDamage(1);
            //gameObject.SetActive(false);
            //StartCoroutine(Respawn());
        }
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
}
