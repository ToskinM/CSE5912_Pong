using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerMovement : MonoBehaviour, IMovement
{
    public Actions Action { get; set; }
    public bool Jumping { get; set; }
    public TerrainCollider terrain;
    private float moveSpeed;
    public float runSpeed;
    public float walkSpeed;
    public float rotateSpeed = 20f;
    public float jumpTimer = 0;
    public bool isAnai;
    public GameObject pickupArea;
    public Rigidbody body;
    private bool onGround = true;
    private FollowCamera cameraScript;

    //follow variables
    private BoxCollider boxCollider;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 4f;
    public GameObject otherCharacter;
    public float smoothTime = 2f;

    private Quaternion rotation = Quaternion.identity;

    void Awake()
    {
        terrain = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
        body = GetComponent<Rigidbody>();
        cameraScript = Camera.main.GetComponent<FollowCamera>();
    }

    void Start()
    {
        Physics.gravity = new Vector3(0, -88.3f, 0);
        walkSpeed = 3f;
        runSpeed = 7f;
        moveSpeed = walkSpeed;
        pickupArea.SetActive(false);

        Action = Actions.Chilling;
        Jumping = false; 
    }

    private void Update()
    {
        // Damping
        body.velocity *= 0.98f;
    }

    void SetWalkOrRun()
    {
        if (Input.GetButtonDown("Run"))
        {
            Action = Actions.Running;
            moveSpeed = runSpeed;
        }
        else
        {
            if (Action != Actions.Running || Input.GetButtonUp("Run"))
            {
                Action = Actions.Walking;
                moveSpeed = walkSpeed;
            }
        }
    }

    void SetJump()
    {
        if (Input.GetButtonDown("Jump") && onGround)
        {
            //Action = Actions.Chilling;
            Jumping = true;
            onGround = false;
            body.AddForce(new Vector3(0f, 25, 0f), ForceMode.Impulse);

        }
        else if (onGround)
        {
            Jumping = false;
        }
    }

    void DetectDodgeInput(Vector3 direction)
    {
        if (Input.GetButtonDown("Block"))
        {
            body.velocity = Vector3.zero;
            body.AddRelativeForce(direction, ForceMode.VelocityChange);
            body.velocity = Vector3.zero;
        }
    }

    void RotateCameraFree()
    {
        // Only rotate with camera while we have movement input
        if (Action != Actions.Chilling)
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    void RotateCameraLocked()
    {
        // look at lock on target
        Vector3 relative = cameraScript.lockOnTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0, angle, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    void MovePlayer(float horizontalInput, float verticalInput)
    {
        // Move foreward in the direction of input
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
        transform.Translate(new Vector3(0f, 0f, Vector3.Magnitude(direction)) * Time.deltaTime * moveSpeed);
    }


    void DetectKeyInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // If we're LOCKED ON
        if (cameraScript.lockOnTarget != null && Action != Actions.Running)
        {
            HandleLockonMovement(verticalInput, horizontalInput);
        }
        // If we're NOT locked on  OR we're Locked on + sprinting (also when the camera is resetting behind the player)
        else
        {
            HandleFreeMovement(verticalInput, horizontalInput);
        }
    }

    void HandleLockonMovement(float verticalInput, float horizontalInput)
    {
        RotateCameraLocked();

        if (verticalInput != 0f)
        {
            SetWalkOrRun();

            DetectDodgeInput(new Vector3(0f, 0f, Mathf.Sign(verticalInput) * 10f));

            Vector3 vertDirection = new Vector3(0, 0, Mathf.Sign(verticalInput));
            transform.Translate(vertDirection * Time.deltaTime * moveSpeed);
        }

        if (horizontalInput != 0f)
        {
            SetWalkOrRun();

            DetectDodgeInput(new Vector3(Mathf.Sign(horizontalInput) * 30f, 0f, 0f));

            Vector3 horiDirection = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
            transform.Translate(horiDirection * Time.deltaTime * moveSpeed);
        }

        SetJump();

        if (Mathf.Approximately(horizontalInput + verticalInput, 0f))
        {
            if (Action != Actions.Chilling) Action = Actions.Chilling;
        }
    }

    void HandleFreeMovement(float verticalInput, float horizontalInput)
    {
        if (verticalInput != 0f || horizontalInput != 0f)
        {
            //Quaternion rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

            SetWalkOrRun();

            DetectDodgeInput(new Vector3(0f, 0f, 30f));
        
            // Dertermine angle we should face from input angle
            float angle = Mathf.Atan2(horizontalInput, verticalInput) * (180 / Mathf.PI);
            angle += cameraScript.transform.rotation.eulerAngles.y;
            //Debug.Log(angle);
            rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
        else
        {
            if (Action != Actions.Chilling) Action = Actions.Chilling;
        }

        SetJump();
        RotateCameraFree();
        MovePlayer(horizontalInput, verticalInput);
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.Equals(terrain))
        {
            onGround = true;
        }
    }
    public void MovementUpdate()
    {
        DecidePickable();
        if (!cameraScript.freeRoam && !cameraScript.switching)
        {
            DetectKeyInput();
        }
        
    }
}
