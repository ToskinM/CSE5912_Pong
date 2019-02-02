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
    private PlayerMovement movement;
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
        if (combatController.inCombat)
        {
            animator.SetBool(key_isAttack02, true);
        }
        else
        {
            animator.SetBool(key_isAttack02, false);
        }
        if (movement.walking)
        {
            animator.SetBool(key_isWalk, true);
        }
        else
        {
            animator.SetBool(key_isWalk, false);
        }
        if (movement.running)
        {
            print("SETRUN");
            animator.SetBool(key_isRun, true);
        }
        else
        {
            animator.SetBool(key_isRun, false);
        }
    }
}
