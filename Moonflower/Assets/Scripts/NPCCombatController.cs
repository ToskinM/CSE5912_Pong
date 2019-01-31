using System.Collections;
using UnityEngine;

public class NPCCombatController : MonoBehaviour
{
    public enum Aggression { Passive, Unaggressive, Aggressive, Frenzied };
    public Aggression aggression;

    public bool hasWeaponOut;
    public bool inCombat;
    public bool isBlocking;
    public bool isAttacking;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter. Tag = " + other.tag);
        if (other.tag == "WeaponHitbox")
        {
            gameObject.SetActive(false);
            //StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(true);
    }
}
