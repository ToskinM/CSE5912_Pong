using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public GameObject ball;
    private float speed = 9f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(speed);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = (ball.transform.position - transform.position).y;
        Vector3 targetLocation = new Vector3(transform.position.x, ball.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);
    }
}
