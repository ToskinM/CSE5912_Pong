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
    public float sneakSpeed;
    public float rotateSpeed = 20f;
    public float jumpTimer = 0;
    public bool isAnai;
    public Rigidbody body;
    private float blockCooldownTime = 0.5f;
    private float blockCooldown;
    private bool blockOffCooldown;
    private bool onGround = true;
    private FollowCamera cameraScript;
    private AudioManager audioManager;

    //follow variables
    private BoxCollider boxCollider;
    const float bufferRadius = 5f;
    const float tooCloseRadius = 4f;
    public GameObject otherCharacter;
    public float smoothTime = 2f;

    private Quaternion rotation = Quaternion.identity;

    public delegate void MakeNoise();
    public static event MakeNoise NoiseRaised;

    void Awake()
    {
        terrain = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
        body = GetComponent<Rigidbody>();
        cameraScript = Camera.main.GetComponent<FollowCamera>();
    }

    void Start()
    {
        Physics.gravity = new Vector3(0, -88.3f, 0);
        walkSpeed = 7f;
        runSpeed = 15f;
        sneakSpeed = 4f;
        moveSpeed = walkSpeed;
        blockCooldown = blockCooldownTime;
        blockOffCooldown = true;

        Action = Actions.Chilling;
        Jumping = false;

        GameStateController.OnPaused += HandlePauseEvent;
    }

    private void Update()
    {
        // Damping
        body.velocity *= 0.98f;
    }

    void SetMovementState()
    {
        if (Input.GetButton("Run") && Action != Actions.Sneaking)
        {
            if (Action != Actions.Running)
            {
                Action = Actions.Running;
                moveSpeed = runSpeed;
            }
        }
        else if (Input.GetButton("Crouch") && Action != Actions.Running)
        {
            if (Action != Actions.Sneaking)
            {
                Action = Actions.Sneaking;
                moveSpeed = sneakSpeed;
            }
        }
        else
        {
            //if (WalkConditionCheck())
            {
                Action = Actions.Walking;
                moveSpeed = walkSpeed;
                //GetComponent<PlayerSoundEffect>().PlayWalkingSFX();
            }
        }
    }

    bool WalkConditionCheck()
    {
        bool releaseRun = Action == Actions.Running && Input.GetButtonUp("Run");
        bool releaseSneak = Action == Actions.Sneaking && Input.GetButtonUp("Crouch");
        bool neutralState = Action != Actions.Running && Action != Actions.Sneaking;

        return releaseRun || releaseSneak || neutralState;
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
        if (Input.GetButtonDown("Block") && blockOffCooldown)
        {
            body.velocity = Vector3.zero;
            body.AddRelativeForce(direction, ForceMode.VelocityChange);
            body.velocity = Vector3.zero;

            blockCooldown = blockCooldownTime;
            blockOffCooldown = false;
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
            SetMovementState();

            DetectDodgeInput(new Vector3(0f, 0f, Mathf.Sign(verticalInput) * 10f));

            Vector3 vertDirection = new Vector3(0, 0, Mathf.Sign(verticalInput));
            transform.Translate(vertDirection * Time.deltaTime * moveSpeed);
        }

        if (horizontalInput != 0f)
        {
            SetMovementState();

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

            SetMovementState();

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

    void UpdateCooldowns()
    {
        UpdateBlockCooldown();
    }

    void UpdateBlockCooldown()
    {
        if (!blockOffCooldown)
        {
            blockCooldown -= Time.deltaTime;

            if (blockCooldown <= 0f) blockOffCooldown = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.Equals(terrain))
        {
            if (!onGround)
            {
                onGround = true;
                MakeNoiseUponLanding();
            }
        }
    }

    public void MovementUpdate()
    {
        if (!cameraScript.freeRoam && !cameraScript.switching)
        {
            DetectKeyInput();
            UpdateCooldowns();
        }
    }

    void MakeNoiseUponLanding()
    {
        if (NoiseRaised != null)
            NoiseRaised();
    }

    // Disable player movement controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }
}
