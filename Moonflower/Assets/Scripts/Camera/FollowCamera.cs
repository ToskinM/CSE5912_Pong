using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [HideInInspector] public Transform lockOnTarget;
    public float followDistanceMultiplier = 1f;
    public float rotateSpeed = 5f;
    public GameObject lockonIndicator;
    public Transform switchTransform;
    [HideInInspector] public bool freeRoam;
    public bool Frozen { get; set; } = false;
    public float switchTime = 0.3f;
    [HideInInspector] public bool switching;
    public bool accountForCollision = true;
    public LayerMask collisionLayers;

    private Transform target;
    private Transform targetCombatTransform;

    private bool lockedOn;

    private Quaternion rotation = Quaternion.identity;
    public float xRotation;
    public float yRotation;
    private Vector3 offset;

    private readonly float freeRoamMoveSpeed = 0.4f;
    private readonly float yRotationMax = 60f;
    private readonly float yRotationMin = -15f;
    private readonly float followDistanceMax = 2f;
    private readonly float followDistanceMin = 0.5f;
    private readonly float collisionOffsetMultiplier = 0.5f;

    private void Awake()
    {
        // Get player target
        target = GetCameraTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    void Start()
    {
        // Get a base distance from player from starting positions
        transform.position = target.position + new Vector3(0, 1, -5);
        offset = target.position - transform.position;

        LevelManager.current.mainCamera = this;
        GameStateController.OnPaused += HandlePauseEvent;
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
                    xRotation += Input.GetAxis("Mouse X") * rotateSpeed;
                    yRotation += -Input.GetAxis("Mouse Y") * rotateSpeed;
                    if (!freeRoam)
                        yRotation = Mathf.Clamp(yRotation, yRotationMin, yRotationMax);
                    else
                        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
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
                rotation = Quaternion.Euler(yRotation, xRotation, 0);
            }
            else if (!switching && lockedOn && lockOnTarget != null)
            {
                Vector3 relative = lockOnTarget.transform.position - target.position;
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                rotation = Quaternion.Euler(yRotation, angle, 0);
            }

            // Find new camera position, Staying behind player, in range
            Vector3 newPosition = target.position - (rotation * offset * followDistanceMultiplier);

            if (accountForCollision)
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
            
            // update position
            transform.position = newPosition;

            // Look at the target
            transform.LookAt(target);
        }
    }
    private void UpdateFreeRotation()
    {
        transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
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
        Transform targetInView = gameObject.GetComponent<FieldOfView>().focusedTarget;
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
    }
    private void LockOff()
    {
        // unlock if locked on
        if (lockedOn)
        {
            // Adjust xRotation to match where we're currently looking (so it doesn't snap back to the pre-lockOn direction)
            xRotation = xRotation + (transform.eulerAngles.y - xRotation);

            lockedOn = false;
            lockOnTarget = null;
            ToggleLockonIndicator(false);
        }
    }
    private IEnumerator ResetCamera()
    {
        // Reset camera behind player
        lockedOn = true;

        float currentAngleY = transform.eulerAngles.y;
        float currentAngleX = transform.eulerAngles.x;

        float startingXRotation = xRotation;
        float startingYRotation = yRotation;

        // Smoothly reset the camera behind player
        float t = 0;
        while (t < switchTime)
        {
            xRotation = Mathf.LerpAngle(xRotation, startingXRotation + (target.eulerAngles.y - currentAngleY), t);
            yRotation = Mathf.LerpAngle(yRotation, startingYRotation + (15f - currentAngleX), t);

            t += Time.deltaTime * 5;
            yield return null;
        }

        lockedOn = false;
    }

    private void SwitchTarget()
    {
        LockOff();
        StartCoroutine(MoveCameraToNewTarget(GetCameraTarget(GameObject.FindGameObjectWithTag("Player"))));
    }
    private IEnumerator MoveCameraToNewTarget(Transform newTarget)
    {
        switching = true;
        switchTransform.position = target.position;
        Vector3 startingPosition = target.position;
        target = switchTransform;

        float currentAngleY = transform.eulerAngles.y;
        float startingXRotation = xRotation;

        // Interpolate target position and xRotation to new target
        float t = 0;
        while (t < switchTime)
        {
            xRotation = Mathf.LerpAngle(startingXRotation, startingXRotation + (newTarget.eulerAngles.y - currentAngleY), t / switchTime);
            switchTransform.position = Vector3.Lerp(startingPosition, newTarget.position, t / switchTime);

            t += Time.deltaTime * 1;
            yield return null;
        }

        target = newTarget;
        switchTransform.position = transform.position;
        switching = false;
    }

    private void ToggleLockonIndicator(bool enable)
    {
        if (enable)
        {
            // place indicator above target
            lockonIndicator.SetActive(true);
            lockonIndicator.transform.parent = lockOnTarget;
            lockonIndicator.transform.localPosition = new Vector3(0, 1.5f, 0);
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
        enabled = !isPaused;
    }
}
