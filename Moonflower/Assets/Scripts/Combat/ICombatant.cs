using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPCCombatController;

public interface ICombatController
{
    CharacterStats Stats { get; }
    bool IsBlocking { get; }
    bool InCombat { get; }
    bool IsDead { get; }
    bool HasWeaponOut { get; }
    GameObject CombatTarget { get; }
    NPCCombatController.Aggression AggressionLevel { get; }
    NPCMovementController Movement { get; set; }
    float AttackDistance { get; }
    NPCGroup Group { get; set; }

    event AggroUpdate OnAggroUpdated;
    event DeathUpdate OnDeath;

    void Stagger();
    void Aggro(GameObject aggroTarget, bool forceAggression);
    void Subdue();
    void AcknowledgeHaveHit(GameObject whoWeHit);
}
