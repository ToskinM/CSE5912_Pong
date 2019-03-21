using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventForwarder : MonoBehaviour
{
    public void EnableHurtbox(int index)
    {
        PlayerController.instance.ActivePlayerAnimator.EnableHurtbox(index);
    }
    public void DisableHurtbox(int index)
    {
        PlayerController.instance.ActivePlayerAnimator.DisableHurtbox(index);
    }
    public void KickJump()
    {
        PlayerController.instance.ActivePlayerMovementControls.KickJump();
    }
    public void SetStunned(int stunned)
    {
        PlayerController.instance.ActivePlayerCombatControls.SetStunned(stunned);
    }
    public void RagdollReplace(int anaiOrMimbi)
    {
        PlayerController.instance.ActivePlayerCombatControls.RagdollReplace(anaiOrMimbi);
    }

    public void GoHurt()
    {
        PlayerController.instance.ActivePlayerCombatControls.GoHurt();
    }
}
