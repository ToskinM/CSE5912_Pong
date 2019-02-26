using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    public Transform target;
    public float switchTime = 0.3f;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(MoveCameraToNewTarget(GetCameraTarget(GameObject.FindGameObjectWithTag("Player"))));
    }

    void Update()
    {
        
    }

    private IEnumerator MoveCameraToNewTarget(Transform newTarget)
    {
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
    }
}
