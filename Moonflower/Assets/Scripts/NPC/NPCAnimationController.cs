using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    private const string key_IsHit = "IsHit";
    private const string key_Attack = "Attack";

    private Animator animator;
    private NPCCombatController combatController;
    private NPCMovement movement;

    public GameObject[] attackHurtboxes;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<NPCMovement>();
        combatController = GetComponent<NPCCombatController>();
    }

    void Update()
    {
        //CleanHurtboxes();
    }

    public void SetHit(int hit)
    {
        if (hit == 1)
        {
            animator.SetBool(key_IsHit, true);

        }
        else
        {
            animator.SetBool(key_IsHit, false);
        }
    }
    public void SetAttack(int attack)
    {
        animator.SetInteger(key_Attack, attack);
        combatController.attack = attack;
    }
    public int GetAttack()
    {
        return animator.GetInteger(key_Attack);
    }

    public void EnableHurtbox(int index)
    {
        attackHurtboxes[index].SetActive(true);
    }
    public void DisableHurtbox(int index)
    {
        attackHurtboxes[index].SetActive(false);
    }

    private void CleanHurtboxes()
    {
        for (int i = 0; i < attackHurtboxes.Length; i++)
        {
        }
    }
}