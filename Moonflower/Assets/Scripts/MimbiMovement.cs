using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class MimbiMovement : MonoBehaviour
{
    private float moveSpeed;
    public GameObject Anai;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public float rotateSpeed = 15f;
    public bool playing;

    NavMeshAgent agent;
    //const float hanginRadius = 15f;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 4f;
    //bool engaging = false;
    //NPCMovement npc;
    //NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        playing = false;
        moveSpeed = 5f;
        agent = GetComponent<NavMeshAgent>(); 
        //targetObject = GameObject.FindGameObjectWithTag("Pyayer");
        //if (targetObject==null)
        //{
        //    Debug.Log("can't find anai");
        //}
    }
    void KeyInput()
    {

    }
    // Update is called once per frame
    void Update()
    {

        print("Anai's position" + Anai.transform.position);
        print("Mimbi's pos" + this.transform.position);
            float distFromPlayer = Vector3.Distance(Anai.transform.position, transform.position);
            if (distFromPlayer < bufferRadius)
            {
                if (distFromPlayer < tooCloseRadius)
                {
                    agent.isStopped = true;
                    Vector3 targetDirection = transform.position - Anai.transform.position;
                    transform.Translate(-targetDirection.normalized * agent.speed * 2 * Time.deltaTime);

                }
                else
                {
                    agent.isStopped = true;
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(Anai.transform.position);
            }
        
    
    }
        


}
