using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatController
{
    CharacterStats Stats { get; }
    bool IsBlocking { get; }
    bool InCombat { get; }
    bool IsDead { get; }
    bool HasWeaponOut { get; }

    void Stagger();
    void AcknowledgeHaveHit(GameObject whoWeHit);
}
