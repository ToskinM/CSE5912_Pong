using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Script should be attached to any entity that involves stat comparison (eg Anai/Mimbi, enemies etc)
    // In case of NPC's, stats will serve as requirement checks against player's stats to determine success of an action

    // Combat-related stats
    public int Strength;        // +25% to weapon base damage per level
    public int Attack;
    public int Defense;         // Reduces incoming damage by 1% per level
    public int MaxHealth;
    public int CurrentHealth;

    // Non-combat related stats
    public int Cunning;
    public int Charisma;
    public int Stealth;

    public float strengthExperience;

    public GameObject HUD;
    private PlayerHealthDisplay display;

    private const float STRENGTH_EXPERIENCE_HIT = 2.5f;
    private const float STRENGTH_EXPERIENCE_KILL = 20f; // This will hopefully vary by character killed

    // Only applicable to player object
    private int companionHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Data drive specific values for each separate entity
        //if (HUD.GetComponent<PlayerHealthDisplay>())
        if (HUD == null)
            HUD = LevelManager.current.dummyHUD;
        display = HUD.GetComponent<PlayerHealthDisplay>();

        companionHealth = CurrentHealth;
        PlayerController.OnCharacterSwitch += SwapPlayerHealth;
    }

    // Not sure how combat/interactions are going to be implemented beforehand (script-wise) so just leaving general methods for now

    public int DealDamage()
    {
        return Attack;
    }

    public void TakeDamage(int damage, string sourceName)
    {
        CurrentHealth -= damage;
        if (display != null)
            display.HitHealth(CurrentHealth, MaxHealth); 
        //Debug.Log(gameObject.name + " took <color=red>" + damage + "</color> damage from " + sourceName);
    }
    public void TakeDamage(int damage, string sourceName, CharacterStats sourceCharacterStats, Vector3 hitPoint, bool blocked)
    {
        if (!blocked)
        {
            CurrentHealth -= DefenceCalculation(damage);

            if (display != null)
                display.HitHealth(CurrentHealth, MaxHealth);
            //Debug.Log(gameObject.name + " took <color=red>" + damage + "</color> damage from " + sourceName);

            if (hitPoint != Vector3.zero)
                ObjectPoolController.current.CheckoutTemporary((GameObject)Resources.Load("Effects/HitEffect_Damage"), hitPoint, 1);
        }
        else
        {
            if (hitPoint != Vector3.zero)
                ObjectPoolController.current.CheckoutTemporary((GameObject)Resources.Load("Effects/HitEffect_Blocked"), hitPoint, 1);
        }

        if (CurrentHealth <= 0)
            sourceCharacterStats.TrainStrengthKill();
        else
            sourceCharacterStats.TrainStrengthHit();
    }

    public bool CompareCunning(int otherCunning)
    {
        return Cunning >= otherCunning;
    }

    public bool CompareCharisma(int otherCharisma)
    {
        return Charisma >= otherCharisma;
    }

    public bool CompareStealth(int otherStealth)
    {
        return Stealth >= otherStealth;
    }
    public void AddHealth (int amount)
    {
        CurrentHealth = CurrentHealth + amount;
        if (CurrentHealth >= MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public void TrainStrengthHit()
    {
        strengthExperience += STRENGTH_EXPERIENCE_HIT;
        StrengthLevelUpCheck();
    }
    public void TrainStrengthKill()
    {
        strengthExperience += STRENGTH_EXPERIENCE_KILL;
        StrengthLevelUpCheck();
    }
    private void StrengthLevelUpCheck()
    {
        if (strengthExperience >= (Strength + 1) * 5)
        {
            strengthExperience = 0;
            Strength++;
        }
    }

    private int DefenceCalculation(int damage)
    {
        return (int)Mathf.Ceil( damage * (Mathf.Clamp((100f - Defense) / 100f, 0f, 1f)) );
    }

    // This method only applies to the player for switching health pools between characters
    void SwapPlayerHealth(PlayerController.PlayerCharacter activeChar)
    {
        if (gameObject.Equals(PlayerController.instance.gameObject)) // Check if the object this script is attached to is the player objet
        {
            int temp = CurrentHealth;
            CurrentHealth = companionHealth;
            companionHealth = temp;
        }
    }
}
