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
    private Camera camera;

    //follow variables
    private BoxCollider boxCollider;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 4f;
    public GameObject otherCharacter;
    public float smoothTime = 2f;

    private Quaternion rotation = Quaternion.identity;

    // Variables to allow GetAxis to behave like GetKeyDown
    private bool jumpAxisInUse;
    private bool runAxisInUse;
    private bool dashAxisInUse;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
        Physics.gravity = new Vector3(0, -88.3f, 0);
        body = GetComponent<Rigidbody>();
        walkSpeed = 3f;
        runSpeed = 7f;
        moveSpeed = walkSpeed;
        pickupArea.SetActive(false);

        Action = Actions.Chilling;
        Jumping = false; 

        this.camera = Camera.main;

        jumpAxisInUse = false;
        runAxisInUse = false;
        dashAxisInUse = false;
    }

    private void Update()
    {
        // Damping
        body.velocity *= 0.98f;
    }

    void SetWalkOrRun()
    {
        if (Input.GetAxisRaw("Run") != 0f && !runAxisInUse)
        {
            Action = Actions.Running;
            moveSpeed = runSpeed;
            runAxisInUse = true;
        }
        else
        {
            if (Action != Actions.Running || (Input.GetAxisRaw("Run") == 0f && runAxisInUse))
            {
                Action = Actions.Walking;
                moveSpeed = walkSpeed;
                runAxisInUse = false;
            }
        }
    }

    void SetJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
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
        if (Input.GetAxisRaw("Dodge") != 0f && !dashAxisInUse)
        {
            body.velocity = Vector3.zero;
            body.AddRelativeForce(direction, ForceMode.VelocityChange);
            body.velocity = Vector3.zero;

            dashAxisInUse = true;
        }
        else if (Input.GetAxisRaw("Dodge") == 0f && dashAxisInUse)
        {
            dashAxisInUse = false;
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
        Vector3 relative = camera.GetComponent<FollowCamera>().lockOnTarget.transform.position - transform.position;
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
        //float jumpInput = Input.GetAxis("Jump");

        // If we're LOCKED ON
        if (camera.GetComponent<FollowCamera>().lockOnTarget != null && Action != Actions.Running)
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
            angle += camera.transform.rotation.eulerAngles.y;
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
        if (!camera.GetComponent<FollowCamera>().freeRoam && !camera.GetComponent<FollowCamera>().switching)
        {
            DetectKeyInput();
        }
        
    }
}
