using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerMovementController : MonoBehaviour
{
    public enum Actions { Walking, Running, Chilling, Sneaking };
    public Actions Action { get; set; }

    public bool Jumping { get; set; }
    public bool Stunned { get; set; }
    public float MovementSpeed;

    private Rigidbody body;
    private FollowCamera cameraScript;

    private bool onGround;
    private bool returnGrav = false;
    private Quaternion rotation = Quaternion.identity;
    private float rotateSpeed = 5f;
    private float blockCooldownTime = 0.5f;
    private float blockCooldown;
    private bool blockOffCooldown;

    const float tooCloseRadius = 2f;
    float followDist = 8f;
    float wanderRadius = 15f;

    private readonly float gravityConstant = 88.3f;

    private readonly float anaiWalkSpeed = 6f;
    private readonly float anaiRunSpeed = 11f;
    private readonly float anaiSneakSpeed = 4f;

    private readonly float mimbiWalkSpeed= 6f;
    private readonly float mimbiRunSpeed = 11f;
    private readonly float mimbiSneakSpeed = 4f;

    public NPCMovementController AnaiPassiveController;
    public NPCMovementController MimbiPassiveController;
    public NPCMovementController CompanionMovementController;

    private PlayerController playerController;
    public GameObject playerObject;

    public delegate void MakeNoise();
    public static event MakeNoise NoiseRaised;

    void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -gravityConstant, 0);
        cameraScript = LevelManager.current.mainCamera;

        SetInitialPlayer();
        SetInitialCompanion();
        SetInitialPlayerState();

        PlayerColliderListener.CollideWithTerrain += HandleTerrainCollision;
        PlayerController.OnCharacterSwitch += SetActivePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        // Damping
        body.velocity *= 0.98f;

       if (!Stunned) DetectKeyInput(); // Controls active player character movement
    }

    // Sets the active object depending on the active character so that this script knows which object to update
    void SetActivePlayer(PlayerCharacter activeChar)
    {
        playerObject = playerController.GetActivePlayerObject();
        body = playerObject.GetComponent<Rigidbody>();
        CompanionMovementController = activeChar == PlayerCharacter.Anai ? MimbiPassiveController : AnaiPassiveController;
    }

    // Detect movement input using vertical and horizontal axes
    void DetectKeyInput()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // If we're locked onto a target
        if (cameraScript.lockOnTarget != null && Action != Actions.Running)
        {
            HandleLockonMovement(verticalInput, horizontalInput);
        }
        else // If we're NOT locked on / are locked on + sprinting
        {
            HandleFreeMovement(verticalInput, horizontalInput);
        }
    }

    // Set player's movement state and speed to Walking
    void SetToWalk(PlayerCharacter activeCharacter)
    {
        Action = Actions.Walking;
        MovementSpeed = activeCharacter == PlayerCharacter.Anai ? anaiWalkSpeed : mimbiWalkSpeed;
    }

    // Set player's movement state and speed to Running
    void SetToRun(PlayerCharacter activeCharacter)
    {
        Action = Actions.Running;
        MovementSpeed = activeCharacter == PlayerCharacter.Anai ? anaiRunSpeed : mimbiRunSpeed;
    }

    // Set player's movement state and speed to Sneaking
    void SetToSneak(PlayerCharacter activeCharacter)
    {
        Action = Actions.Sneaking;
        MovementSpeed = activeCharacter == PlayerCharacter.Anai ? anaiSneakSpeed : mimbiSneakSpeed;
    }

    // If there is movement input, update the movement states
    void SetMovementState()
    {
        if (Input.GetButton("Run") && Action != Actions.Sneaking)
        {
            if (Action != Actions.Running) SetToRun(playerController.GetActiveCharacter());
        }
        else if (Input.GetButton("Crouch") && Action != Actions.Running)
        {
            if (Action != Actions.Sneaking) SetToSneak(playerController.GetActiveCharacter());
        }
        else
        {
            SetToWalk(playerController.GetActiveCharacter());
        }
    }

    // Handle player movement when player is locked onto a target
    void HandleLockonMovement(float verticalInput, float horizontalInput)
    {
        RotateCameraLocked();

        if (verticalInput != 0f)
        {
            SetMovementState();

            DetectDodgeInput(new Vector3(0f, 0f, Mathf.Sign(verticalInput) * 10f));

            Vector3 vertDirection = new Vector3(0, 0, Mathf.Sign(verticalInput));
            playerObject.transform.Translate(vertDirection * Time.deltaTime * MovementSpeed);
        }

        if (horizontalInput != 0f)
        {
            SetMovementState();

            DetectDodgeInput(new Vector3(Mathf.Sign(horizontalInput) * 30f, 0f, 0f));

            Vector3 horiDirection = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
            playerObject.transform.Translate(horiDirection * Time.deltaTime * MovementSpeed);
        }

        SetJump();

        if (Mathf.Approximately(horizontalInput + verticalInput, 0f))
        {
            if (Action != Actions.Chilling) Action = Actions.Chilling;
        }
    }

    // Handle player movement when player is not locked onto a target
    void HandleFreeMovement(float verticalInput, float horizontalInput)
    {
        if (verticalInput != 0f || horizontalInput != 0f)
        {
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

    // Handle player and camera rotation when player is not locked onto a target
    void RotateCameraFree()
    {
        // Only rotate with camera while we have movement input
        if (Action != Actions.Chilling)
        {
            playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
    }

    // Handle player and camera rotation when player is locked onto a target
    void RotateCameraLocked()
    {
        // look at lock on target
        Vector3 relative = cameraScript.lockOnTarget.transform.position - playerObject.transform.position;
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0, angle, 0);

        playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    // Handle player jump movements
    void SetJump()
    {
        if (Input.GetButtonDown("Jump") && onGround)
        {
            Jumping = true;
            onGround = false;
            if (Action != Actions.Running)
            {
                body.AddForce(new Vector3(0f, 30f, 0f), ForceMode.Impulse);
            }
            else
            {
                body.AddForce(new Vector3(0f, 40f, 0f), ForceMode.Impulse);
            }

        }
        else if (onGround)
        {
            Jumping = false;
        }
    }

    // Update all classes subscribed to this event
    void MakeNoiseOnLanding()
    {
        NoiseRaised?.Invoke();
    }

    // Gets called when a player collides with terrain from the ColliderListener in each player object
    void HandleTerrainCollision()
    {
        if (returnGrav)
            Physics.gravity = new Vector3(0, -gravityConstant, 0);

        if (!onGround)
        {
            onGround = true;
            MakeNoiseOnLanding();
        }
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

    void MovePlayer(float horizontalInput, float verticalInput)
    {
        // Move foreward in the direction of input
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
        playerObject.transform.Translate(new Vector3(0f, 0f, Vector3.Magnitude(direction)) * Time.deltaTime * MovementSpeed);
    }

    public void KickJump()
    {
        Physics.gravity = new Vector3(0, -22.1f, 0);
        if (onGround)
        {
            body.AddRelativeForce(Vector3.up * 7, ForceMode.Impulse);
            onGround = false;
        }
        returnGrav = true;
    }

    private void SetAnaiPassiveMovement()
    {
        AnaiPassiveController = new NPCMovementController(playerController.AnaiObject, playerController.MimbiObject);
        AnaiPassiveController.FollowPlayer(followDist, tooCloseRadius);
    }

    private void SetMimbiPassiveMovement()
    {
        MimbiPassiveController = new NPCMovementController(playerController.MimbiObject, playerController.AnaiObject);
        MimbiPassiveController.WanderFollowPlayer(wanderRadius);
        MimbiPassiveController.SetDefault(NPCMovementController.MoveState.wanderfollow);
    }

    public void UpdateCompanionMovement(PlayerCharacter activeChar)
    {
        if (activeChar == PlayerCharacter.Anai)
        {
            MimbiPassiveController.UpdateMovement();
        }
        else
        {
            AnaiPassiveController.UpdateMovement();
        }
    }

    void SetInitialPlayerState()
    {
        Action = Actions.Chilling;
        Jumping = false;
        Stunned = false;
        onGround = true;
        blockCooldown = blockCooldownTime;
        blockOffCooldown = true;
        SetToWalk(playerController.GetActiveCharacter());
    }

    void SetInitialPlayer()
    {
        playerObject = playerController.GetActivePlayerObject();
        body = playerObject.GetComponent<Rigidbody>();
    }

    void SetInitialCompanion()
    {
        SetAnaiPassiveMovement();
        SetMimbiPassiveMovement();
        CompanionMovementController = playerController.GetActiveCharacter() == PlayerCharacter.Anai ? MimbiPassiveController : AnaiPassiveController;
    }
}
