using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [HideInInspector] public Transform lockOnTarget;
    public float followDistanceMultiplier = 1f;
    public float rotateSpeed = 5f;
    public GameObject lockonIndicator;
    private Transform switchTransform;
    [HideInInspector] public bool freeRoam;
    public bool Frozen { get; set; } = false;
    public float switchTime = 1f;
    [HideInInspector] public bool switching;
    public bool accountForCollision = true;
    public LayerMask collisionLayers;

    private Camera camera;
    private FieldOfView fieldOfView;
    private AudioListener audioListener;

    private Transform target;
    private Transform targetCombatTransform;

    private bool lockedOn;

    private float shakeAmount;

    private Quaternion rotation = Quaternion.identity;
    [HideInInspector] public float yRotation;
    [HideInInspector] public float xRotation;
    private Vector3 offset;

    private readonly float freeRoamMoveSpeed = 0.4f;
    private readonly float yRotationMax = 60f;
    private readonly float yRotationMin = -15f;
    private readonly float followDistanceMax = 2f;
    private readonly float followDistanceMin = 0.5f;
    public float collisionOffsetMultiplier = -0.8f;

    public delegate void LockonUpdate(GameObject target);
    public event LockonUpdate OnLockon;

    public delegate void LockoffUpdate();
    public event LockoffUpdate OnLockoff;

    private void Awake()
    {
        // Get player target
        target = GetCameraTarget(GameObject.FindGameObjectWithTag("Player"));
        camera = GetComponent<Camera>();
        audioListener = GetComponent<AudioListener>();
        fieldOfView = GetComponent<FieldOfView>();
    }

    public void SetRendering(bool rendering)
    {
        camera.enabled = rendering;
        audioListener.enabled = rendering;
    }

    void Start()
    {
        // Get a base distance from player from starting positions
        transform.position = target.position + new Vector3(0, 1, -5);
        offset = target.position - transform.position;

        //LevelManager.current.mainCamera = this;
    }

    private void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;
        PlayerController.instance.ActivePlayerCombatControls.OnHit += HandleCameraShake;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;
        PlayerController.instance.ActivePlayerCombatControls.OnHit -= HandleCameraShake;
    }

    void Update()
    {
        if (!switching)
        {
            // Switch to correct character (Anai or Mimbi)
            if (target.root.tag != "Player")
            {
                SwitchTarget();
            }


            // Lock off if lock on target is no longer active
            if (lockOnTarget != null && lockedOn && !lockOnTarget.gameObject.activeInHierarchy)
            {
                LockOff();
            }
            // Rotation adjustment
            if (!Frozen)
            {
                if (!Input.GetButtonDown("LockOn") && !switching)
                {
                    yRotation += Input.GetAxis("Mouse X") * rotateSpeed;
                    xRotation += -Input.GetAxis("Mouse Y") * rotateSpeed;
                    if (!freeRoam)
                        xRotation = Mathf.Clamp(xRotation, yRotationMin, yRotationMax);
                    else
                        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                    yRotation = yRotation % 360;
                }
            }


            // Follow distance adjustment
            followDistanceMultiplier += -Input.GetAxis("Mouse ScrollWheel");
            followDistanceMultiplier = Mathf.Clamp(followDistanceMultiplier, followDistanceMin, followDistanceMax);
        }
    }

    void LateUpdate()
    {
        if (freeRoam)
        {
            // Free-roam camera
            UpdateFreeMovement();
            UpdateFreeRotation();
        }
        else
        {
            if (!switching && Input.GetButtonDown("LockOn") && !freeRoam)
            {
                ManageLockOn();
            }
            if (lockOnTarget == null || switching)
            {
                rotation = Quaternion.Euler(xRotation, yRotation, 0);
            }
            else if (!switching && lockedOn && lockOnTarget != null)
            {
                Vector3 relative = lockOnTarget.transform.position - target.position;
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                rotation = Quaternion.Euler(xRotation, angle, 0);
            }

            // update position
            transform.position = GetNewPosition() + (Random.insideUnitSphere * shakeAmount);

            // Look at the target
            transform.LookAt(target);
        }

        shakeAmount *= 0.9f;
        if (shakeAmount < 0.02f)
        {
            shakeAmount = 0;
        }
    }

    private void HandleCameraShake(GameObject aggressor)
    {
        shakeAmount = 0.3f;
    }

    private Vector3 GetNewPosition()
    {
        // Find new camera position, Staying behind player, in range
        Vector3 newPosition = target.position - (rotation * offset * followDistanceMultiplier);

        if (accountForCollision && !switching)
        {
            // Account for collision with objects in camMask
            RaycastHit wallHit = new RaycastHit();
            //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
            if (Physics.Linecast(target.transform.position, newPosition, out wallHit, collisionLayers))
            {
                //the x and z coordinates are pushed away from the wall by hit.normal.
                //the y coordinate stays the same.
                newPosition = new Vector3(wallHit.point.x + wallHit.normal.x * collisionOffsetMultiplier, newPosition.y, wallHit.point.z + wallHit.normal.z * collisionOffsetMultiplier);
            }
        }

        return newPosition;
    }

    private void UpdateFreeRotation()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void UpdateFreeMovement()
    {
        if (Input.GetKey(KeyCode.W)) transform.Translate(0, 0, freeRoamMoveSpeed);
        if (Input.GetKey(KeyCode.S)) transform.Translate(0, 0, -freeRoamMoveSpeed);
        if (Input.GetKey(KeyCode.A)) transform.Translate(-freeRoamMoveSpeed, 0, 0);
        if (Input.GetKey(KeyCode.D)) transform.Translate(freeRoamMoveSpeed, 0, 0);
        if (Input.GetKey(KeyCode.Space)) transform.Translate(0, freeRoamMoveSpeed, 0);
        if (Input.GetKey(KeyCode.LeftControl)) transform.Translate(0, -freeRoamMoveSpeed, 0);
    }

    public void SetFreeRoam(bool enabled)
    {
        freeRoam = enabled;
    }

    public Transform GetCameraTarget(GameObject character)
    {
        // Find gameobject tagged as the camera look target (ONLY searches one hierarchy level deep)
        foreach (Transform transform in character.transform)
        {
            if (transform.tag == "CameraTarget")
            {
                return transform;
            }
        }

        Debug.Log("Failed to get camera target.");
        return null;
    }

    private void ManageLockOn()
    {
        Transform targetInView = fieldOfView.focusedTarget;
        if (lockedOn && lockOnTarget != null)
        {
            LockOff();
        }
        else if (targetInView != null && !lockedOn)
        {
            LockOn(targetInView);
        }
        else
        {
            StartCoroutine(ResetCamera());
        }
    }
    private void LockOn(Transform targetToLockTo)
    {
        // lock on to a target
        lockedOn = true;

        lockOnTarget = targetToLockTo;
        ToggleLockonIndicator(true);

        OnLockon?.Invoke(targetToLockTo.gameObject);
    }
    public void LockOff()
    {
        // unlock if locked on
        if (lockedOn)
        {
            // Adjust xRotation to match where we're currently looking (so it doesn't snap back to the pre-lockOn direction)
            yRotation = yRotation + (transform.eulerAngles.y - yRotation);

            lockedOn = false;
            lockOnTarget = null;
            ToggleLockonIndicator(false);

            OnLockoff?.Invoke();
        }
    }
    private IEnumerator ResetCamera()
    {
        // Reset camera behind player
        lockedOn = true;

        float currentAngleY = transform.eulerAngles.y;
        float currentAngleX = transform.eulerAngles.x;

        float startingXRotation = yRotation;
        float startingYRotation = xRotation;

        // Smoothly reset the camera behind player
        for (float t = 0; t < switchTime; t += Time.deltaTime * 1.5f)
        {
            yRotation = Mathf.LerpAngle(yRotation, target.eulerAngles.y, t / switchTime);
            xRotation = Mathf.LerpAngle(xRotation, startingYRotation + (15f - currentAngleX), t / switchTime);

            yield return null;
        }

        lockedOn = false;
    }

    private void SwitchTarget()
    {
        if (lockedOn)
            LockOff();

        StartCoroutine(MoveCameraToNewTarget(GetCameraTarget(GameObject.FindGameObjectWithTag("Player"))));
    }
    private IEnumerator MoveCameraToNewTarget(Transform newCameraTarget)
    {
        switching = true;
        switchTransform = Instantiate((GameObject)Resources.Load("SwitchTransform")).transform;

        switchTransform.position = target.position;

        target = switchTransform;

        Vector3 startingPosition = target.position;
        //float currentAngleY = transform.eulerAngles.y;
        float startingYRotation = yRotation;

        // Interpolate target position and xRotation to new target
        for (float t = 0; t < switchTime; t += Time.deltaTime)
        {
            switchTransform.position = Vector3.Lerp(startingPosition, newCameraTarget.position, t / switchTime);
            yRotation = Mathf.LerpAngle(startingYRotation, newCameraTarget.eulerAngles.y, t / switchTime);

            yield return null;
        }

        target = newCameraTarget;

        Destroy(switchTransform.gameObject);
        switchTransform = null;
        //switchTransform.position = transform.position;
        switching = false;
    }
    public IEnumerator TransitionFromDialogue(Vector3 startingPosition)
    {
        switching = true;

        Vector3 targetPosition = GetNewPosition();
        //Vector3 startingPosition = transform.position;

        float startingAngleY = transform.eulerAngles.y;

        // Interpolate target position and xRotation to new target
        for (float t = 0; t < switchTime; t += Time.deltaTime * 1)
        {
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, Mathf.LerpAngle(startingAngleY, yRotation, t / switchTime), rotation.eulerAngles.z);
            switchTransform.position = Vector3.Lerp(startingPosition, targetPosition, t / switchTime);

            yield return null;
        }

        switching = false;
    }

    private void ToggleLockonIndicator(bool enable)
    {
        if (enable)
        {
            // place indicator above target
            lockonIndicator.SetActive(true);
            lockonIndicator.transform.parent = lockOnTarget;
            lockonIndicator.transform.localPosition = new Vector3(0, 2f, 0);
            //lockOnTarget.gameObject.GetComponent<Renderer>().bounds.extents.y + 1;
        }
        else
        {
            // take indicator off target and hide it
            lockonIndicator.transform.parent = transform;
            lockonIndicator.SetActive(false);
        }
    }

    // Disable player movement controls when game is paused
    void HandlePauseEvent(bool isPaused)
    {
        //enabled = !isPaused;
    }
}
