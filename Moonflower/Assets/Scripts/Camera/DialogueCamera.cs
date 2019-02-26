using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    public Transform dialogueTarget;
    public Transform dialoguePosition;

    public float switchTime = 0.3f;
    public bool relocating = false;

    void Start()
    {
        LevelManager.current.dialogueCamera = this;
        enabled = false;
    }

    //private void OnDisable()
    //{
    //    StartCoroutine(
    //        PositionCamera(GetCameraTarget(GameObject.FindGameObjectWithTag("Player"))));
    //}

    public void OnEnable()
    {
        dialoguePosition = GetCameraTarget(GameObject.FindGameObjectWithTag("Player"));
        StartCoroutine(PositionCamera(dialoguePosition));
    }
    public void OnDisable()
    {
        dialogueTarget = null;
        //    StartCoroutine(
        //        PositionCamera(GetCameraTarget(GameObject.FindGameObjectWithTag("Player"))));
    }

    void Update()
    {
        if (!relocating)
            transform.position = dialoguePosition.position;

        Vector3 headPosition = dialogueTarget.position + Vector3.up;
        transform.LookAt(headPosition);
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

    private IEnumerator PositionCamera(Transform newTarget)
    {
        relocating = true;
        Vector3 startingPosition = transform.position;
        Vector3 targetPosition = newTarget.position;

        //Interpolate target position to new target
        for (float t = 0; t < switchTime; t += Time.deltaTime * 1)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, t / switchTime);

            yield return null;
        }
        relocating = false;
    }
}
