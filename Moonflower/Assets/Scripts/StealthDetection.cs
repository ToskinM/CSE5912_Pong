using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StealthDetection : MonoBehaviour
{
    private NavMeshAgent nav;
    private SphereCollider col;
    private bool heardJump;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        nav = GetComponent<NavMeshAgent>();
        heardJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            PlayerMovement.NoiseRaised += HandleNoiseRaised;

            if (player.GetComponent<PlayerMovement>().Action == Actions.Walking)
            {
                if (CalculatePathLength(player.transform.position) <= col.radius / 2)
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
            else if (player.GetComponent<PlayerMovement>().Action == Actions.Running || heardJump)
            {
                if (CalculatePathLength(player.transform.position) <= col.radius)
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            PlayerMovement.NoiseRaised -= HandleNoiseRaised;
        }
    }

    float CalculatePathLength(Vector3 targetPosiion)
    {
        NavMeshPath path = new NavMeshPath();

        if (nav.enabled)
        {
            nav.CalculatePath(targetPosiion, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosiion;

        for (int i=0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for(int i=0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

    void HandleNoiseRaised()
    {
        // Cause enemy to hear with greater threshold
    }
}
