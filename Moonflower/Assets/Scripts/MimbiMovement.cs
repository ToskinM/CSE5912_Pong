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
        // Match camera y rotation if moving
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        //{
        //    Quaternion rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        //}

        if (Input.GetKey(KeyCode.W))
        {
            //need change when prefab is changed
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //need change when prefab is changed
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * 10 * Time.deltaTime * rotateSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(-Vector3.up * 10 * Time.deltaTime * rotateSpeed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        {
            playing = !playing;
        }
        if (playing)
        {
            KeyInput();
            //Debug.Log("I AM USING MIMBI");

        }
        else
        {
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
        


}
