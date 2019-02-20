using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StealthDetection : MonoBehaviour
{
    private NavMeshAgent nav;
    private SphereCollider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        nav = GetComponent<NavMeshAgent>();
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

            if (player.GetComponent<PlayerMovement>().Action == Actions.Walking)
            {
                if (CalculatePathLength(player.transform.position) <= col.radius / 2)
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
            else if (player.GetComponent<PlayerMovement>().Action == Actions.Running)
            {
                if (CalculatePathLength(player.transform.position) <= col.radius)
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.cyan;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) gameObject.GetComponent<Renderer>().material.color = Color.white;
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
}
