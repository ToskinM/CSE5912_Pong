using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerutiAnimatorController : MonoBehaviour
{
    JerutiController jerutiController;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animator;

    private const string key_isDistracted = "IsDistracted";
    private const string key_isWalking = "Walking";
    private const string key_isTalking = "Talking";
    private const string key_isWaiting = "Waiting";

    private bool tempbool = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        jerutiController = GetComponent<JerutiController>();
    }

    public void StartDistraction()
    {
        animator.SetBool(key_isDistracted, true);
        DisableWalking();
        DisableTalking();
        DisableWaiting();
        tempbool = true;
    }
    public void EndDistraction()
    {
        animator.SetBool(key_isDistracted, false);
        EnableWalking();
        tempbool = false;
    }

    public void EnableTalking()
    {
        animator.SetBool(key_isTalking, true);
    }
    public void DisableTalking()
    {
        animator.SetBool(key_isTalking, false);
    }
    public void EnableWalking()
    {
        animator.SetBool(key_isWalking, true);
    }
    public void DisableWalking()
    {
        animator.SetBool(key_isWalking, false);
    }
    public void EnableWaiting()
    {
        animator.SetBool(key_isWaiting, true);
    }
    public void DisableWaiting()
    {
        animator.SetBool(key_isWaiting, false);
    }

    public void EngageInDialogue()
    {
        EnableTalking();
        DisableWaiting();
        DisableWalking();
    }

    public void EndDiaglogue()
    {
        DisableTalking();
        EnableWaiting();
        DisableWalking();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude > 0 && tempbool == false)
        {
            animator.SetBool(key_isWalking, true);
            animator.SetBool("Waiting", false);
            animator.SetBool("Talking", false);
        }
        else
        {
            if (jerutiController.dialogueActive)
            {
                animator.SetBool("Talking", true);
                animator.SetBool("Waiting", false);
                animator.SetBool(key_isWalking, false);
            }
            else
            {
                animator.SetBool("Talking", false);
                animator.SetBool("Waiting", true);
                animator.SetBool(key_isWalking, false);
            }
        }

    }
}
