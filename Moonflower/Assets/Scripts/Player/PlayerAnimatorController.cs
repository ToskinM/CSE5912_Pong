using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private const string key_isRun = "IsRun";
    private const string key_isWalk = "IsWalk";
    private const string key_isAttack01 = "IsAttack01";
    private const string key_isAttack02 = "IsAttack02";
    private const string key_AttackTrigger = "Attack";
    private const string key_Attack = "Attack";
    private const string key_isJump = "IsJump";
    private const string key_isDamage = "IsDamage";
    private const string key_isDead = "IsDead";
    private Animator animator;
    private PlayerCombatController combatController;

    public IMovement movement { get; set; } 
    public HurtboxController[] attackHurtboxes;
    public GameObject walkParticles;
    public GameObject runParticles;
    public GameObject standingParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        combatController = GetComponent<PlayerCombatController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (combatController.isAttacking)
        //{
        //    if (combatController.attack == 0)
        //    {
        //        animator.SetBool(key_isAttack02, false);
        //        animator.SetBool(key_isAttack01, true);
        //    }
        //    else if (combatController.attack == 1)
        //    {
        //        animator.SetBool(key_isAttack01, false);
        //        animator.SetBool(key_isAttack02, true);
        //    }
        //}
        //else
        //{
        //    animator.SetBool(key_isAttack01, false);
        //    animator.SetBool(key_isAttack02, false);
        //}

        animator.SetBool(key_isWalk, movement.Action == Actions.Walking);
        animator.SetBool(key_isRun, movement.Action == Actions.Running);
        animator.SetBool(key_isJump, movement.Jumping);

        if (walkParticles || runParticles || standingParticles)
        {
            standingParticles.SetActive(!(movement.Action == Actions.Running || movement.Action == Actions.Walking));
            walkParticles.SetActive(movement.Action == Actions.Walking);
            runParticles.SetActive(movement.Action == Actions.Running);
        }
    }

    public void TriggerAttack()
    {
        animator.SetTrigger(key_AttackTrigger);
    }

    public void EnableHurtbox(int index)
    {
        attackHurtboxes[index].Enable(combatController.GetAttackDamage());
    }
    public void DisableHurtbox(int index)
    {
        attackHurtboxes[index].Disable();
    }
}
