using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEntranceCamera : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float panSpeed = 1;
    [Range(0, 1)] public float recenterTimeMultiplier = 1;
    public float yRotationClamp = 15; // degrees
    public float xRotationClamp = 15; // degrees

    private Transform player;
    private Vector3 playerStartPosition;

    private float cameraPanDistance;

    private float inputRotationX;
    private float inputRotationY;

    void Start()
    {
        // Get max distance for normalizing interpolation 
        cameraPanDistance = Vector3.Distance(startPoint.position, endPoint.position);

        // Get player Info
        player = PlayerController.instance.AnaiObject.transform;
        playerStartPosition = player.position;

        // Go to Start Position
        transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
    }

    void Update()
    {
        // Get input look adjustment
        inputRotationX = Mathf.Clamp(inputRotationX + Input.GetAxis("Mouse Y"), -yRotationClamp, yRotationClamp);
        inputRotationY = Mathf.Clamp(inputRotationY + Input.GetAxis("Mouse X"), -xRotationClamp, xRotationClamp);

        // At what percentage from the start to end should we be on? Based on player's position
        float interpolation = Mathf.Clamp(Vector3.Distance(playerStartPosition, player.position), 0f, cameraPanDistance) / cameraPanDistance;

        // Get the position and rotation at that percentage
        Vector3 newPosition = Vector3.Lerp(startPoint.position, endPoint.position, interpolation);
        Quaternion newRotation = Quaternion.Lerp(startPoint.rotation, endPoint.rotation, interpolation) * Quaternion.Euler(-inputRotationX, inputRotationY, 0f);

        // Smoothly match those values
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * panSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * panSpeed);

        // Smoothly keep camera centered
        inputRotationX *= 0.96f * recenterTimeMultiplier;
        inputRotationY *= 0.96f * recenterTimeMultiplier;
    }
}
