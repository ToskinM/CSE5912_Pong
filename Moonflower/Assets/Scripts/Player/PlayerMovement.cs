using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    public float runSpeed;
    public float walkSpeed;
    public float rotateSpeed = 20f;
    public float jumpTimer = 0;
    [HideInInspector] public bool playing;
    [HideInInspector] public bool jumping;
    public bool isAnai;
    [HideInInspector] public bool walking;
    [HideInInspector] public bool running;
    public GameObject pickupArea;
    public Rigidbody body;

    private Camera camera;

    //follow variables
    private NavMeshAgent agent;
    private BoxCollider boxCollider;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 4f;
    public GameObject otherCharacter;
    public float smoothTime = 2f;

    private Quaternion rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = Physics.gravity * 3;
        body = GetComponent<Rigidbody>();
        moveSpeed = 3f;
        walkSpeed = 3f;
        runSpeed = 7f;
        pickupArea.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();

        if (this.gameObject.name == "Anai")
        {
            playing = true;
            //boxCollider.enabled = true;
            agent.enabled = false;
            this.camera = Camera.main;
        }
        else
        {
            //boxCollider.enabled = false;
            this.camera = null;
            agent.enabled = true;
            playing = false;
        }
    }
    void DetectKeyInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        float jumpInput = Input.GetAxis("Jump");

        // Don't move in camera free roam
        if (!camera.GetComponent<FollowCamera>().freeRoam)
        {
            // If we're LOCKED ON
            if (camera.GetComponent<FollowCamera>().lockOnTarget != null && !running)
            {
                // look at lock on target
                Vector3 relative = camera.GetComponent<FollowCamera>().lockOnTarget.transform.position - transform.position;
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                rotation = Quaternion.Euler(0, angle, 0);

                //rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
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
                        if (!running || Input.GetKeyUp(KeyCode.LeftShift))
                        {
                            walking = true;
                            running = false;
                            moveSpeed = walkSpeed;
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        body.velocity = Vector3.zero;
                        body.AddRelativeForce(new Vector3(0f, 0f, Mathf.Sign(verticalInput) * 10f), ForceMode.VelocityChange);
                        body.velocity = Vector3.zero;
                    }
                    Vector3 vertDirection = new Vector3(0, 0, Mathf.Sign(verticalInput));
                    transform.Translate(vertDirection * Time.deltaTime * moveSpeed);
                }
                if (horizontalInput != 0f)
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        running = true;
                        walking = false;
                        moveSpeed = runSpeed;
                    }
                    else
                    {
                        if (!running || Input.GetKeyUp(KeyCode.LeftShift))
                        {
                            walking = true;
                            running = false;
                            moveSpeed = walkSpeed;
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        body.velocity = Vector3.zero;
                        body.AddRelativeForce(new Vector3(Mathf.Sign(horizontalInput) * 12f, 0f, 0f), ForceMode.VelocityChange);
                        body.velocity = Vector3.zero;
                    }
                    Vector3 horiDirection = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
                    transform.Translate(horiDirection * Time.deltaTime * moveSpeed);
                }
                if (Input.GetKeyDown(KeyCode.Space) && jumpTimer == 0)
                {
                    jumping = true;
                    walking = false;
                    running = false;
                    body.AddForce(new Vector3(0f, 5f, 0f), ForceMode.Impulse);
                    jumpTimer = 40;
                }
                else
                {
                    jumping = false;
                }
                if (Mathf.Approximately(horizontalInput + verticalInput, 0f))
                {
                    walking = false;
                    running = false;
                }
            }
            // If we're NOT locked on  OR we're Locked on + sprinting (also when the camera is resetting behind the player)
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
                        if (!running || Input.GetKeyUp(KeyCode.LeftShift))
                        {
                            walking = true;
                            running = false;
                            moveSpeed = walkSpeed;
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        body.velocity = Vector3.zero;
                        body.AddRelativeForce(new Vector3(0f, 0f, 10f), ForceMode.VelocityChange);
                        body.velocity = Vector3.zero;
                    }

                    // Dertermine angle we should face from input angle
                    float angle = Mathf.Atan2(horizontalInput, verticalInput) * (180 / Mathf.PI);
                    angle += camera.transform.rotation.eulerAngles.y;
                    //Debug.Log(angle);
                    rotation = Quaternion.AngleAxis(angle, Vector3.up);
                }
                else
                {
                    running = false;
                    walking = false;
                }
                if (Input.GetKeyDown(KeyCode.Space) && body.velocity.y == 0)
                {
                    jumping = true;
                    walking = false;
                    running = false;
                    body.AddForce(new Vector3(0f, 25, 0f), ForceMode.Impulse);
                }
                else
                {
                    jumping = false;
                }

                // Only rotate with camera while we have movement input
                if (walking || running)
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

                // Move foreward in the direction of input
                Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
                transform.Translate(new Vector3(0f, 0f, Vector3.Magnitude(direction)) * Time.deltaTime * moveSpeed);
            }
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

    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        {
            playing = !playing;
            if (playing)
            {
                GetComponent<PlayerCombatController>().enabled = true;
                tag = "Player";
                camera = Camera.main;
                //boxCollider.enabled = true;
                agent.enabled = false;

            }
            else
            {
                GetComponent<PlayerCombatController>().enabled = false;
                tag = "Companion";
                agent.enabled = true;
                running = false;
                //boxCollider.enabled = false;
                jumping = false;

            }
        }
        if (playing)
        {
            DecidePickable();
            if (!camera.GetComponent<FollowCamera>().freeRoam && !camera.GetComponent<FollowCamera>().switching)
            {
                DetectKeyInput();
            }
        }
        else
        {
            Follow();
        }
    }

    void Follow()
    {
        float distFromPlayer = Vector3.Distance(otherCharacter.transform.position, transform.position);
        if (distFromPlayer < bufferRadius)
        {
            if (distFromPlayer < tooCloseRadius)
            {
                agent.isStopped = true;
                walking = false;

                Vector3 targetDirection = transform.position - otherCharacter.transform.position;
                transform.Translate(-targetDirection.normalized * 2 * Time.deltaTime);

            }
            else
            {
                agent.isStopped = true;
                walking = false;

            }
        }
        else
        {
            agent.isStopped = false;
            walking = true;
            agent.SetDestination(otherCharacter.transform.position);
        }
    }
}
