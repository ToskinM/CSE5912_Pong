﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaruAnimatorController : MonoBehaviour
{
    AmaruController amaruController;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        amaruController = GetComponent<AmaruController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude > 0)
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Waiting", false);
            animator.SetBool("Talking", false);
        }
        else
        {
            if (amaruController.dialogueActive)
            {
                animator.SetBool("Talking", true);
                animator.SetBool("Waiting", false);
                animator.SetBool("Walking", false);
            }
            else
            {
                animator.SetBool("Talking", false);
                animator.SetBool("Waiting", true);
                animator.SetBool("Walking", false);
            }
        }
    }
}
