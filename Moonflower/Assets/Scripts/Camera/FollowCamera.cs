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
    private readonly float freeRoamRotateSpeed = 2.5f;
    private readonly float freeRoamMoveSpeed = 0.5f;

    private Vector3 offset;
    private Quaternion rotation;

    void Start()
    {
        offset = target.transform.position - transform.position;
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
            if (dampen)
            {
                // Rotate WITH target (Dampened)
                float currentAngle = transform.eulerAngles.y;
                float targetAngle = target.transform.eulerAngles.y;
                float angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * damping);
                rotation = Quaternion.Euler(0, angle, 0);
            }
            else
            {
                // Rotate WITH target
                float angle = target.transform.eulerAngles.y;
                rotation = Quaternion.Euler(0, angle, 0);
            }

            // Stay behind player, in range
            transform.position = target.transform.position - (rotation * offset * followDistanceMultiplier);

            // Look at the targer
            transform.LookAt(target.transform);
        }
    }
    private void UpdateFreeRotation()
    {
        if (Input.GetKey(KeyCode.UpArrow)) transform.Rotate(-freeRoamRotateSpeed, 0, 0);
        if (Input.GetKey(KeyCode.DownArrow)) transform.Rotate(freeRoamRotateSpeed, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Rotate(0, -freeRoamRotateSpeed, 0);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Rotate(0, freeRoamRotateSpeed, 0);
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

        //if (!freeRoam)
        //{
        //    transform.position = defaultPosOffset;
        //    transform.rotation = defaultRotation;
        //}
    }
}
