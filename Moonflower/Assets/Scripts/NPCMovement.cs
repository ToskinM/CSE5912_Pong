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
    const float bufferDist = 0.2f;
    int count = 0;
    int threshhold;
    const int smallPause = 30, largePause = 80;

    private void Start()
    {
        Wandering = true;
        dest = getRandomDest();
        transform.position = dest;
        threshhold = getRandomPause(); 
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dest);
        Debug.Log(transform.position); 

        if (Wandering)
        {
            if (Vector3.Distance(agent.transform.position, dest) <= bufferDist)
            {
                Debug.Log("New Dest");
                dest = getRandomDest();
                agent.SetDestination(dest);
            }
        }
    }

    Vector3 getRandomDest()
    {
        float x = Random.Range(plane.transform.position.x - plane.transform.localScale.x,
            plane.transform.position.x + plane.transform.localScale.x);
        float z = Random.Range(plane.transform.position.z - plane.transform.localScale.z,
            plane.transform.position.z + plane.transform.localScale.z);
        return new Vector3(x, 0, z); 
    }

    int getRandomPause()
    {
        return Random.Range(smallPause, largePause); 
    }
    
}
