using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;

    public bool playing = true;
    public GameObject camera;
    public float rotateSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
    }
    void KeyInput()
    {
        // Match camera y rotation if moving
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Quaternion rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
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
        { playing = !playing; }
        if(playing)
            KeyInput();

        
    }
}
