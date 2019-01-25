using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public GameObject target;
    public float damping = 1f;

    private bool freeRoam;
    private readonly float freeRoamRotateSpeed = 2.5f;
    private readonly float freeRoamMoveSpeed = 0.5f;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    private void LateUpdate()
    {
        if (freeRoam)
        {
            UpdateFreeMovement();
            UpdateFreeRotation();
        }
        else
        {
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
