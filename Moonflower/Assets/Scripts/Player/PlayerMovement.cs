using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    public bool playing = true;
    public GameObject camera;
    public float rotateSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
    }
    void DetectKeyInput()
    {
        // Match camera y rotation if moving
        if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
        {
            Quaternion rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput != 0f)
        {
            Vector3 vertDirection = new Vector3(0, 0, Mathf.Sign(verticalInput));
            transform.Translate(vertDirection * Time.deltaTime * moveSpeed);
        }
        if (horizontalInput != 0f)
        {
            Vector3 horiDirection = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
            transform.Translate(horiDirection * Time.deltaTime * moveSpeed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        { playing = !playing; }
        if(playing)
            DetectKeyInput();
    }
}
