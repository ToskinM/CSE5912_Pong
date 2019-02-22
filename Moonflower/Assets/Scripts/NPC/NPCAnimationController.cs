﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    private const string key_IsWalk = "IsWalk";
    private const string key_IsRun = "IsRun";
    private const string key_IsJump = "IsJump";
    private const string key_Attack = "Attack";
    private const string key_AttackTrigger = "Attack";
    private const string key_IsBlocking = "IsBlocking";
    private const string key_IsHit = "IsHit";
    private const string key_IsStunned = "IsStunned";
    private const string key_IsDead = "IsDead";

    private Animator animator;
    private NPCCombatController combatController;
    private NPCMovement movement;

    public HurtboxController[] attackHurtboxes;

    void Awake()
    {
        animator = GetComponent<Animator>();
        combatController = GetComponent<NPCCombatController>();
    }

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    void Update()
    {
        if (animator && movement)
        {
            animator.SetBool(key_IsWalk, movement.Action == Actions.Walking);
            animator.SetBool(key_IsRun, movement.Action == Actions.Running);
            animator.SetBool(key_IsJump, movement.Jumping);
        }
    }

    public void SetWalk(bool walking)
    {
        animator.SetBool(key_IsWalk, walking);
    }
    public void SetRun(bool running)
    {
        animator.SetBool(key_IsRun, running);
    }

    //public void SetAttack(int attack)
    //{
    //    animator.SetInteger(key_Attack, attack);
    //    combatController.attack = attack;
    //}
    public void TriggerAttack()
    {
        animator.SetTrigger(key_AttackTrigger);
    }
    public int GetAttack()
    {
        return animator.GetInteger(key_Attack);
    }
    public void TriggerHit()
    {
        animator.SetTrigger(key_IsHit);
    }
    public void SetIsDead(bool isDead)
    {
        animator.SetBool(key_IsDead, isDead);
    }

    public void EnableHurtbox(int index)
    {
        attackHurtboxes[index].Enable(combatController.GetAttackDamage());
    }
    public void DisableHurtbox(int index)
    {
        attackHurtboxes[index].Disable();
    }

    private void CleanHurtboxes()
    {
        for (int i = 0; i < attackHurtboxes.Length; i++)
        {
        }
    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }
}