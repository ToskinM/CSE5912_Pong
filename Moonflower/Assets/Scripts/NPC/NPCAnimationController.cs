using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    private const string key_isHit = "IsHit";

    private Animator animator;
    private NPCCombatController combatController;
    private NPCMovement movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<NPCMovement>();
        combatController = GetComponent<NPCCombatController>();
    }

    void Update()
    {
    }

    public void SetHit(int hit)
    {
        if (hit == 1)
        {
            animator.SetBool(key_isHit, true);
        }
        else
        {
            animator.SetBool(key_isHit, false);
        }
    }
}
