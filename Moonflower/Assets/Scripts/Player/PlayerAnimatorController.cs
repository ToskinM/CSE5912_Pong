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
    private const string key_isBlock = "IsBlock";
    private const string key_isDistract = "IsCute";

    private Animator animator;
    private Animator companionAnimator;
    public PlayerCombatController combatController;
    private PlayerController playerController;
    private PlayerMovementController playerMovement;

    public HurtboxController[] anaiAttackHurtboxes;
    public HurtboxController[] mimbiAttackHurtboxes;
    public GameObject walkParticles;
    public GameObject runParticles;
    public GameObject standingParticles;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovementController>();
        combatController = GetComponent<PlayerCombatController>();

        animator = playerController.GetActivePlayerObject().GetComponent<Animator>();
        companionAnimator = playerController.GetCompanionObject().GetComponent<Animator>();
    }

    void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;
        PlayerController.OnCharacterSwitch += SetActiveCharacter;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(key_isBlock, combatController.IsBlocking);
        animator.SetBool(key_isWalk, playerMovement.Action == PlayerMovementController.Actions.Walking);
        animator.SetBool(key_isRun, playerMovement.Action == PlayerMovementController.Actions.Running);
        animator.SetBool(key_isJump, playerMovement.Jumping);
        animator.SetBool(key_isCrouch, playerMovement.Action == PlayerMovementController.Actions.Sneaking);

        if (walkParticles || runParticles || standingParticles)
        {
            standingParticles.SetActive(!(playerMovement.Action == PlayerMovementController.Actions.Running || playerMovement.Action == PlayerMovementController.Actions.Walking));
            walkParticles.SetActive(playerMovement.Action == PlayerMovementController.Actions.Walking);
            runParticles.SetActive(playerMovement.Action == PlayerMovementController.Actions.Running);
        }
    }

    public void TriggerAttack()
    {
        // The jump on kick is now called by an animation event

        animator.SetTrigger(key_AttackTrigger);
    }

    public void TriggerHit()
    {
        animator.SetTrigger(key_IsHitTrigger);
    }

    public void EnableDistraction()
    {
        animator.SetBool(key_isDistract, true);
    }

    public void DisableDistraction()
    {
        animator.SetBool(key_isDistract, false);
    }

    public void TriggerDeath()
    {
        animator.SetBool(key_isDead, true);
        //animator.SetTrigger(key_isDead);
    }

    public void EnableHurtbox(int index)
    {
        if (playerController.GetActiveCharacter() == PlayerController.PlayerCharacter.Anai)
        {
            anaiAttackHurtboxes[index].Enable(PlayerController.instance.ActivePlayerCombatControls.GetAttackDamage(index));
        }
        else
        {
            mimbiAttackHurtboxes[index].Enable(PlayerController.instance.ActivePlayerCombatControls.GetAttackDamage(index));
        }
    }

    public void DisableHurtbox(int index)
    {
        if (playerController.GetActiveCharacter() == PlayerController.PlayerCharacter.Anai)
        {
            anaiAttackHurtboxes[index].Disable();
        }
        else
        {
            mimbiAttackHurtboxes[index].Disable();
        }
    }

    // Disable player animation when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        //animator.enabled = !isPaused;
    }

    public void SetActiveCharacter(PlayerController.PlayerCharacter activeChar)
    {
        animator = playerController.GetActivePlayerObject().GetComponent<Animator>();
        companionAnimator = playerController.GetCompanionObject().GetComponent<Animator>();
    }

    // Called in the PlayerController
    public void UpdateCompanionAnimation(PlayerController.PlayerCharacter activeChar)
    {
        companionAnimator.SetBool(key_isWalk, playerMovement.CompanionMovementController.Action == Actions.Walking);
        companionAnimator.SetBool(key_isRun, playerMovement.CompanionMovementController.Action == Actions.Running);
        companionAnimator.SetBool(key_isJump, playerMovement.CompanionMovementController.Jumping);
        companionAnimator.SetBool(key_isCrouch, playerMovement.CompanionMovementController.Action == Actions.Sneaking);
    }
}
