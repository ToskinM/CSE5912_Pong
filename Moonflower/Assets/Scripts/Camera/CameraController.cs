using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 defaultPosOffset;
    private Quaternion defaultRotation;
    private bool freeRoam;
    private readonly float rotateSpeed = 2.5f;
    private readonly float moveSpeed = 0.5f;

    // Use this for initialization
    void Start()
    {
        freeRoam = false;
        defaultPosOffset = transform.position - player.transform.position;
        defaultRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (freeRoam)
        //{
        //    UpdateFreeMovement();
        //    UpdateFreeRotation();
        //}
        //else
        //{
        //    transform.position = player.transform.position + defaultPosOffset;
        //}
    }

    private void UpdateFreeRotation()
    {
        if (Input.GetKey(KeyCode.UpArrow)) transform.Rotate(-rotateSpeed, 0, 0);
        if (Input.GetKey(KeyCode.DownArrow)) transform.Rotate(rotateSpeed, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Rotate(0, -rotateSpeed, 0);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Rotate(0, rotateSpeed, 0);
    }

    private void UpdateFreeMovement()
    {
        if (Input.GetKey(KeyCode.W)) transform.Translate(0, 0, moveSpeed);
        if (Input.GetKey(KeyCode.S)) transform.Translate(0, 0, -moveSpeed);
        if (Input.GetKey(KeyCode.A)) transform.Translate(-moveSpeed, 0, 0);
        if (Input.GetKey(KeyCode.D)) transform.Translate(moveSpeed, 0, 0);
        if (Input.GetKey(KeyCode.Space)) transform.Translate(0, moveSpeed, 0);
        if (Input.GetKey(KeyCode.LeftControl)) transform.Translate(0, -moveSpeed, 0);
    }

    public void SetFreeRoam(bool enabled)
    {
        freeRoam = enabled;

        if (!freeRoam)
        {
            transform.position = defaultPosOffset;
            transform.rotation = defaultRotation;
        }
    }

}
