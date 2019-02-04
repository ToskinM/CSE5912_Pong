using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    public float runSpeed;
    public float walkSpeed;
    public float rotateSpeed = 20f;
    public float jumpTimer = 0;
    public bool playing;
    public GameObject camera;
    public bool jumping;
    public bool walking;
    public bool running;
    public GameObject pickupArea;

    private Quaternion rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        playing = true;
        moveSpeed = 3f;
        walkSpeed = 3f;
        runSpeed = 7f;
        pickupArea.SetActive(false);
    }
    void DetectKeyInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        float jumpInput = Input.GetAxis("Jump");
        // If we're LOCKED ON
        if (camera.GetComponent<FollowCamera>().lockOnTarget != null)
        {
            rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

            if (verticalInput != 0f)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    running = true;
                    walking = false;
                    moveSpeed = runSpeed;
                }
                else
                {
                    if (!running ||Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        walking = true;
                        running = false;
                        moveSpeed = walkSpeed;
                    }

                }
                Vector3 vertDirection = new Vector3(0, 0, Mathf.Sign(verticalInput));
                transform.Translate(vertDirection * Time.deltaTime * moveSpeed);
            }
            if (horizontalInput != 0f)
            {   if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    running = true;
                    walking = false;
                    moveSpeed = runSpeed;
                }
                else
                {
                    if (!running ||Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        walking = true;
                        running = false;
                        moveSpeed = walkSpeed;
                    }

                }
                Vector3 horiDirection = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
                transform.Translate(horiDirection * Time.deltaTime * moveSpeed);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumping = true;
                walking = false;
                running = false;
            } else
            {
                jumping = false;
            }
        }
        // If we're NOT locked on (also when the camera is resetting behind the player)
        else
        {
            if (verticalInput != 0f || horizontalInput != 0f)
            {
                //Quaternion rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
                //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    running = true;
                    walking = false;
                    moveSpeed = runSpeed;
                }
                else
                {
                    if (!running ||Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        walking = true;
                        running = false;
                        moveSpeed = walkSpeed;
                    }

                }
                float angle = Mathf.Atan2(horizontalInput, verticalInput) * (180 / Mathf.PI);
                angle += camera.transform.rotation.eulerAngles.y;
                //Debug.Log(angle);
                rotation = Quaternion.AngleAxis(angle, Vector3.up);
            }else{
                running = false;
                walking = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumping = true;
                walking = false;
                running = false;
                jumpTimer = 100;
            }
            else
            {
                jumping = false;
            }

           /* if(jumpTimer != 0)
            {
                print("help");
                if(jumpTimer > 50)
                {
                    transform.Translate(new Vector3(0f, 1000f, 0f) * Time.deltaTime);
                } else
                {
                    transform.Translate(new Vector3(0f, -1000f, 0f) * Time.deltaTime);
                }
                jumpTimer--;
            }
            */
             transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
            transform.Translate(new Vector3(0f, 0f, Vector3.Magnitude(direction)) * Time.deltaTime * moveSpeed);
        }
    }
    public void DecidePickable()
    {
        if (Input.GetButton("Pickup"))
        {
            pickupArea.SetActive(true);
        }
        if (Input.GetButtonUp("Pickup"))
        {
            pickupArea.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        { playing = !playing; }
        if (playing)
        {
            DecidePickable();
            DetectKeyInput();
        }
    }
}
