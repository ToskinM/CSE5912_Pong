using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private const string key_isRun = "IsRun";
    private const string key_isWalk = "IsWalk";
    private const string key_AttackTrigger = "Attack";
    private const string key_IsHitTrigger = "IsHit";
    private const string key_Attack = "Attack";
    private const string key_isJump = "IsJump";
    private const string key_isDead = "IsDead";
    private const string key_isCrouch = "IsCrouch";
    private Animator animator;
    private PlayerCombatController combatController;

    public IMovement movement { get; set; } 
    public HurtboxController[] attackHurtboxes;
    public GameObject walkParticles;
    public GameObject runParticles;
    public GameObject standingParticles;
    public PlayerMovement playerMovement { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        combatController = GetComponent<PlayerCombatController>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        GameStateController.OnPaused += HandlePauseEvent;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(key_isWalk, movement.Action == Actions.Walking);
        animator.SetBool(key_isRun, movement.Action == Actions.Running);
        animator.SetBool(key_isJump, movement.Jumping);
        animator.SetBool(key_isCrouch, movement.Action == Actions.Sneaking);

        if (walkParticles || runParticles || standingParticles)
        {
            standingParticles.SetActive(!(movement.Action == Actions.Running || movement.Action == Actions.Walking));
            walkParticles.SetActive(movement.Action == Actions.Walking);
            runParticles.SetActive(movement.Action == Actions.Running);
        }
    }

    public void TriggerAttack()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            playerMovement.KickJump();
        }

        animator.SetTrigger(key_AttackTrigger);
    }
    public void TriggerHit()
    {
        animator.SetTrigger(key_IsHitTrigger);
    }

    public void EnableHurtbox(int index)
    {
        attackHurtboxes[index].Enable(combatController.GetAttackDamage(index));
    }

    public void DisableHurtbox(int index)
    {
        attackHurtboxes[index].Disable();
    }


    // Disable player animation when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        animator.enabled = !isPaused;
    }
}
