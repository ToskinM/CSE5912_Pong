using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestController : MonoBehaviour
{
    private float speed = 11f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            velocity.y = speed;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            velocity.y = -speed;
        }
        if (Input.GetKey(KeyCode.J))
        {
            rotation.z = 4;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            rotation.z = -4;
        }
        transform.Rotate(rotation);
        transform.position += velocity * Time.deltaTime;
    }
}
