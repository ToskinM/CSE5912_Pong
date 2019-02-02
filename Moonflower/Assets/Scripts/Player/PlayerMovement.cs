using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed = 20f;
    public bool playing;
    public GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        playing = true;
        moveSpeed = 5f;
    }
    void DetectKeyInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // Match camera y rotation if moving

        Quaternion rotation = Quaternion.identity;
        if (camera.GetComponent<FollowCamera>().lockOnTarget != null)
        {
            rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

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
        else
        {
            if (verticalInput != 0f || horizontalInput != 0f)
            {
                //Quaternion rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
                //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

                float angle = Mathf.Atan2(horizontalInput, verticalInput) * (180 / Mathf.PI);
                angle += camera.transform.rotation.eulerAngles.y;
                //Debug.Log(angle);
                rotation = Quaternion.AngleAxis(angle, Vector3.up);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
            transform.Translate(new Vector3(0f, 0f, Vector3.Magnitude(direction)) * Time.deltaTime * moveSpeed);
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
