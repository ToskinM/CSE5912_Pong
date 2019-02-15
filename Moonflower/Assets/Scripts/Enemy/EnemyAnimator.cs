using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private const string key_Attack = "ToAttack";
    private const string key_Defend = "DefendTrigger";
    private const string key_GetHit = "GetHitTrigger";
    private const string key_Walk = "WalkForwardTrigger";
    private const string key_Run = "RunForwardTigger";
    private const string key_Die = "ToDie";
    private const string key_Dizzy = "DizzyTrigger";


    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAttack()
    {
        animator.SetBool(key_Attack, true);
    }
    public void SetWalk()
    {
        animator.SetTrigger(key_Walk);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
