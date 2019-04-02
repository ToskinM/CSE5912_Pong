using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatantTempMovement : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 15f;

    private Vector3 velocity = Vector3.zero;
    private NavMeshAgent agent;
    private ICombatController npcCombatController;
    private Vector3 startPosition;

    const float bufferRadius = 2.5f;
    const float tooCloseRadius = 2f;
    const float deaggroRadius = 10f;

    void Start()
    {
        startPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        npcCombatController = GetComponent<ICombatController>();
    }
    void KeyInput()
    {

    }
    void Update()
    {
        target = npcCombatController.CombatTarget;
        if (target)
        {
            float distFromPlayer = Vector3.Distance(target.transform.position, transform.position);
            if (distFromPlayer < bufferRadius)
            {
                if (distFromPlayer < tooCloseRadius)
                {
                    agent.isStopped = true;
                    Vector3 targetDirection = target.transform.position - transform.position;
                    transform.Translate(targetDirection.normalized * agent.speed * 20 * Time.deltaTime);
                }
                else
                {
                    agent.isStopped = true;
                }
            }
            else
            {
                if (distFromPlayer < deaggroRadius)
                {
                    agent.isStopped = false;
                    agent.ResetPath();
                    agent.SetDestination(target.transform.position);
                }
                else
                {
                    agent.isStopped = false;
                    agent.ResetPath();
                    agent.SetDestination(startPosition);
                }
            }

            // look at lock on target
            Vector3 relative = target.transform.position - transform.position;
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
    }
}
