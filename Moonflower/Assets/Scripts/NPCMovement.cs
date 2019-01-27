using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class NPCMovement : MonoBehaviour
{
    public bool Wandering { get; set; }

    public NavMeshAgent agent;
    public GameObject plane;

    Vector3 dest;
    const float bufferDist = .2f;
    int count = 0;
    int threshhold;
    const int smallPause = 1, largePause = 5;

    private void Start()
    {
        Wandering = true;
        dest = getRandomDest();
        transform.position = dest;
        threshhold = getRandomPause(); 
    }

    void Update()
    {
        if (Wandering)
        {
            bool atDest = getXZDist(agent.transform.position, dest) <= bufferDist;
            if (atDest && !agent.isStopped)
            {
                agent.isStopped = true;
            }
            else if(atDest)
            {

                if (count == threshhold)
                {
                    agent.isStopped = false; 
                    dest = getRandomDest();
                    agent.SetDestination(dest);

                    threshhold = getRandomPause();
                    count = 0; 
                }
                else
                {
                    count++; 
                }

            }
        } 
    }

    private Vector3 getRandomDest()
    {
        Renderer planeRend = plane.GetComponent<Renderer>();
        Vector3 extent = planeRend.bounds.extents;
        float x = planeRend.bounds.center.x + Random.Range(-extent.x, extent.x);
        float z = planeRend.bounds.center.z + Random.Range(-extent.z, extent.z); 
        return new Vector3(x, 0, z); 
    }

    private int getRandomPause()
    {
        return Random.Range(smallPause*60, largePause*60); 
    }

    private float getXZDist(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z);
    
    }


}
