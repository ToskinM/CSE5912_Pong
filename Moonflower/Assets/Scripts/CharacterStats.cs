using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Script should be attached to any entity that involves stat comparison (eg Anai/Mimbi, enemies etc)
    // In case of NPC's, stats will serve as requirement checks against player's stats to determine success of an action

    // Combat-related stats
    public int Strength;
    public int Attack;
    public int Defense;
    public int MaxHealth;
    public int CurrentHealth;

    // Non-combat related stats
    public int Cunning;
    public int Charisma;
    public int Stealth;

    // Start is called before the first frame update
    void Start()
    {
        // Data drive specific values for each separate entity
    }

    // Not sure how combat/interactions are going to be implemented beforehand (script-wise) so just leaving general methods for now

    public void DealDamage()
    {
        // 
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    public void CompareCunning()
    {

    }

    public void CompareCharisma()
    {

    }

    public void CompareStealth()
    {

    }
}
