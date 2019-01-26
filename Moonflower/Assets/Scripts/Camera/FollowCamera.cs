using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public float followDistanceMultiplier = 1f;
    public float rotateSpeed = 5f;
    public float damping = 1f;
    public bool dampen = false;

    private bool freeRoam;
    private Vector3 offset;
    private Quaternion rotation;
    private float xRotation;
    private float yRotation;

    private readonly float freeRoamMoveSpeed = 0.4f;
    private readonly float yRotationMax = 60f;
    private readonly float yRotationMin = -10f;
    private readonly float followDistanceMax = 3f;
    private readonly float followDistanceMin = 0.5f;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void Update()
    {
        // Rotation adjustment
        xRotation += Input.GetAxis("Mouse X") * rotateSpeed;
        yRotation += -Input.GetAxis("Mouse Y") * rotateSpeed;
        if (!freeRoam)
            yRotation = Mathf.Clamp(yRotation, yRotationMin, yRotationMax);

        // Follow distance adjustment
        followDistanceMultiplier += -Input.GetAxis("Mouse ScrollWheel");
        followDistanceMultiplier = Mathf.Clamp(followDistanceMultiplier, followDistanceMin, followDistanceMax);
    }

    void LateUpdate()
    {
        if (freeRoam)
        {
            UpdateFreeMovement();
            UpdateFreeRotation();
        }
        else
        {
            rotation = Quaternion.Euler(yRotation, xRotation, 0);

            //// If we want camera locked to player's rotation
            //if (dampen)
            //{
            //    // Rotate WITH target (Dampened)
            //    float currentAngle = transform.eulerAngles.y;
            //    float targetAngle = target.transform.eulerAngles.y;
            //    float angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * damping);
            //    rotation = Quaternion.Euler(0, angle, 0);
            //}
            //else
            //{
            //    // Rotate WITH target
            //    float angle = target.transform.eulerAngles.y;
            //    rotation = Quaternion.Euler(0, angle, 0);
            //}

            // Stay behind player, in range
            transform.position = target.transform.position - (rotation * offset * followDistanceMultiplier);

            // Look at the targer
            transform.LookAt(target.transform);
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
}
