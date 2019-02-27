using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private NavMeshAgent agent;
    private NPCCombatController combatController;
    private LesserNPCController controller;
    private NPCMovement movement;

    public HurtboxController[] attackHurtboxes;

    void Awake()
    {
        //animator = GetComponent<Animator>();
        animator = GetComponentInChildren<Animator>();
        combatController = GetComponent<NPCCombatController>();
        controller = GetComponent<LesserNPCController>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }

    void Update()
    {
        // Not sure why this won't work
        //if (animator && movement)
        //{
        //    animator.SetBool(key_IsWalk, movement.Action == Actions.Walking);
        //    animator.SetBool(key_IsRun, movement.Action == Actions.Running);
        //    animator.SetBool(key_IsJump, movement.Jumping);
        //}

        if (agent)
        {
            if (agent.isStopped)
            {
                animator.SetBool(key_IsRun, false);
                animator.SetBool(key_IsWalk, false);
                return;
            }

            if (agent.velocity.magnitude > agent.speed * (2f/3f)) // Run
            {
                animator.SetBool(key_IsRun, true);
                animator.SetBool(key_IsWalk, false);
            }
            else if (agent.velocity.magnitude > 0.1f)  // Walk
            {
                animator.SetBool(key_IsRun, false);
                animator.SetBool(key_IsWalk, true);
            }
            else                                    // Idle
            {
                animator.SetBool(key_IsRun, false);
                animator.SetBool(key_IsWalk, false);
            }
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

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }
}