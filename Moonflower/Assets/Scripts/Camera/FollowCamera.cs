using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform targetTransform;
    [HideInInspector] public Transform lockOnTarget;
    public float followDistanceMultiplier = 1f;
    public float rotateSpeed = 5f;
    public bool useCombatAngle;
    //public float damping = 1f;
    //public bool dampen = false;

    private Transform target;
    private Transform targetCombatTransform;
    private PlayerMovement playerMovement;
    private bool freeRoam;
    private bool lockedOn;
    private bool mouseInput;
    private Vector3 offset;
    private Quaternion rotation = Quaternion.identity;
    private float xRotation;
    private float yRotation;

    private readonly float freeRoamMoveSpeed = 0.4f;
    private readonly float yRotationMax = 60f;
    private readonly float yRotationMin = -10f;
    private readonly float followDistanceMax = 3f;
    private readonly float followDistanceMin = 0.5f;

    private void Awake()
    {
        // Get player movement script
        playerMovement = gameObject.GetComponent<PlayerMovement>();

        target = targetTransform;
        if (useCombatAngle)
        {
            // Find the target to use in combat
            foreach (Transform transform in targetTransform)
            {
                if (transform.tag == "CameraCombatTarget")
                {
                    targetCombatTransform = transform;
                    break;
                }
            }
        }
    }

    void Start()
    {
        // Get a base distance from player from starting positions
        transform.position = target.position + new Vector3(0, 1, -5);
        offset = target.position - transform.position;
    }

    void Update()
    {
        // Rotation adjustment
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            mouseInput = true;
        else
            mouseInput = false;
        //Debug.Log(Input.GetAxis("Mouse X"));

        if (!Input.GetButtonDown("LockOn")) { 
            xRotation += Input.GetAxis("Mouse X") * rotateSpeed;
            yRotation += -Input.GetAxis("Mouse Y") * rotateSpeed;
            if (!freeRoam)
                yRotation = Mathf.Clamp(yRotation, yRotationMin, yRotationMax);
        }


        // Follow distance adjustment
        followDistanceMultiplier += -Input.GetAxis("Mouse ScrollWheel");
        followDistanceMultiplier = Mathf.Clamp(followDistanceMultiplier, followDistanceMin, followDistanceMax);
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
            //if (playerMovement != null)
            //{
            //    Debug.Log("playerMovement found");

            //}

            // If we want camera locked to player's rotation
            //if (!mouseInput && movementInput)
            //{
            //    // Rotate WITH target (Dampened)
            //    float currentAngle = transform.eulerAngles.y;
            //    float targetAngle = target.transform.eulerAngles.y;
            //    float angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * damping);
            //    rotation = Quaternion.Euler(0, angle, 0);
            //}
            //else
            //{
            //rotation = Quaternion.Euler(yRotation, xRotation, 0);
            //}

            if (Input.GetButtonDown("LockOn") && !freeRoam)
            {
                //rotation = Quaternion.Euler(0, target.transform.eulerAngles.y, 0);
                StartCoroutine(LockOn());
            }
            //else
            //{
            //if (!lockedOn)
            //{
            if (lockOnTarget == null)
            {
                rotation = Quaternion.Euler(yRotation, xRotation, 0);
            }
            else
            {
                //if (lockOnTarget != null)
                //{
                    Vector3 relative = lockOnTarget.transform.position - target.position;
                    float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                    rotation = Quaternion.Euler(yRotation, angle, 0);
                //}
            }
            //}
            //}

            // Stay behind player, in range
            transform.position = target.position - (rotation * offset * followDistanceMultiplier);

            // Look at the targer
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

    private IEnumerator LockOn()
    {
        Transform targetInView = gameObject.GetComponent<FieldOfView>().focusedTarget;
        if (lockedOn && lockOnTarget != null)
        {
            // unlock if locked on
            //Debug.Log("Unlock");

            // Adjust xRotation to match where we're currently looking (so it doesn't snap back to the pre-lockOn direction)
            xRotation = xRotation + (transform.eulerAngles.y - xRotation);

            if (useCombatAngle)
                target = targetTransform;
            lockedOn = false;
            lockOnTarget = null;
        }
        else if (targetInView != null && !lockedOn)
        {
            // lock on to a target
            //Debug.Log("Lock");
            lockedOn = true;

            if (useCombatAngle)
                target = targetCombatTransform;
            lockOnTarget = targetInView;
        }
        else
        {
            // Reset camera behind player
            lockedOn = true;

            float currentAngleY = transform.eulerAngles.y;
            float currentAngleX = transform.eulerAngles.x;
            float targetAngleY = target.eulerAngles.y;

            float x = xRotation;
            float y = yRotation;

            // Smoothly reset the camera behind player
            float t = 0;
            while (t < 1)
            {
                xRotation = Mathf.LerpAngle(xRotation, x + (targetAngleY - currentAngleY), t);
                yRotation = Mathf.LerpAngle(yRotation, y + (15f - currentAngleX), t);

                t += Time.deltaTime * 5;
                yield return null;
            }

            lockedOn = false;
        }
    }
}
