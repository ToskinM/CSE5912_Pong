using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsCamera : MonoBehaviour
{
    public float panSpeed = 1;
    [Range(0, 1)] public float recenterTimeMultiplier = 1;
    public float yRotationClamp = 5; // degrees
    public float xRotationClamp = 5; // degrees

    public GameObject clickProjectile;
    public Transform projectileNode;

    private float inputRotationX;
    private float inputRotationY;

    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        // Get input look adjustment
        inputRotationX = Mathf.Clamp(inputRotationX + Input.GetAxis("Mouse Y"), -yRotationClamp, yRotationClamp);
        inputRotationY = Mathf.Clamp(inputRotationY + Input.GetAxis("Mouse X"), -xRotationClamp, xRotationClamp);

        // Get the position and rotation at that percentage
        Quaternion newRotation = Quaternion.Euler(-inputRotationX, inputRotationY, 0f);

        // Smoothly match those values
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * panSpeed);

        // Smoothly keep camera centered
        inputRotationX *= 0.96f * recenterTimeMultiplier;
        inputRotationY *= 0.96f * recenterTimeMultiplier;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitPoint = hit.point;

                LaunchProjectile(hitPoint);
            }
        }
    }

    public void LaunchProjectile(Vector3 target)
    {
        GameObject projectile = Instantiate(clickProjectile, projectileNode.position, Quaternion.LookRotation(target - transform.position));
        projectile.transform.LookAt(target);
    }
}
