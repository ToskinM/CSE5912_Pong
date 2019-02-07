﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform targetTransform;
    [HideInInspector] public Transform lockOnTarget;
    public float followDistanceMultiplier = 1f;
    public float rotateSpeed = 5f;
    public bool useCombatAngle;
    public GameObject lockonIndicator;
    [HideInInspector] public bool freeRoam;

    private PlayerMovement playerMovement;

    private Transform target;
    private Transform targetCombatTransform;

    private bool lockedOn;

    private Quaternion rotation = Quaternion.identity;
    private float xRotation;
    private float yRotation;
    private Vector3 offset;

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
        if (lockedOn && !lockOnTarget.gameObject.activeInHierarchy)
        {
            LockOff();
        }

        // Rotation adjustment
        if (!Input.GetButtonDown("LockOn"))
        {
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
            if (Input.GetButtonDown("LockOn") && !freeRoam)
            {
                ManageLockOn();
            }
            if (lockOnTarget == null)
            {
                rotation = Quaternion.Euler(yRotation, xRotation, 0);
            }
            else
            {
                Vector3 relative = lockOnTarget.transform.position - target.position;
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                rotation = Quaternion.Euler(yRotation, angle, 0);
            }

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
            ResetCamera();
        }
    }
    private void LockOn(Transform targetToLockTo)
    {
        // lock on to a target
        lockedOn = true;

        if (useCombatAngle)
            target = targetCombatTransform;
        lockOnTarget = targetToLockTo;
        ToggleLockonIndicator(true);
    }
    private void LockOff()
    {
        // unlock if locked on

        // Adjust xRotation to match where we're currently looking (so it doesn't snap back to the pre-lockOn direction)
        xRotation = xRotation + (transform.eulerAngles.y - xRotation);

        if (useCombatAngle)
            target = targetTransform;
        lockedOn = false;
        lockOnTarget = null;
        ToggleLockonIndicator(false);
    }
    private IEnumerator ResetCamera()
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

    private void ToggleLockonIndicator(bool enable)
    {
        if (enable)
        {
            // place indicator above target
            lockonIndicator.SetActive(true);
            lockonIndicator.transform.parent = lockOnTarget;
            lockonIndicator.transform.localPosition = new Vector3(0, 1, 0);
        }
        else
        {
            // take indicator off target and hide it
            lockonIndicator.transform.parent = transform;
            lockonIndicator.SetActive(false);
        }

    }
}
