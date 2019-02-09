using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private const string key_isRun = "IsRun";
    private const string key_isWalk = "IsWalk";
    private const string key_isAttack01 = "IsAttack01";
    private const string key_isAttack02 = "IsAttack02";
    private const string key_isJump = "IsJump";
    private const string key_isDamage = "IsDamage";
    private const string key_isDead = "IsDead";

    private Animator animator;
    private PlayerCombatController combatController;

    public IMovement movement { get; set; } 
    public GameObject[] attackHurtboxes;

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
        if (combatController.isAttacking)
        {
            if (combatController.attack == 0)
            {
                animator.SetBool(key_isAttack02, false);
                animator.SetBool(key_isAttack01, true);
            }
            else if (combatController.attack == 1)
            {
                animator.SetBool(key_isAttack01, false);
                animator.SetBool(key_isAttack02, true);
            }
        }
        else
        {
            animator.SetBool(key_isAttack01, false);
            animator.SetBool(key_isAttack02, false);
        }

        animator.SetBool(key_isWalk, movement.Action == Actions.Walking);
        animator.SetBool(key_isRun, movement.Action == Actions.Running);
        animator.SetBool(key_isJump, movement.Jumping);
    }

    public void EnableHurtbox(int index)
    {
        attackHurtboxes[index].SetActive(true);
    }
    public void DisableHurtbox(int index)
    {
        attackHurtboxes[index].SetActive(false);
    }
}
