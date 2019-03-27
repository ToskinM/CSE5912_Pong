using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    private enum DialogueCameraState { Dormant, Transitioning, Active }
    private DialogueCameraState state;

    [HideInInspector] public Transform dialogueTarget;
    [HideInInspector] public Transform dialoguePosition;

    public float switchTime = 0.5f;

    private Transform playerTransform;

    private Camera camera;
    private AudioListener audioListener;
    GameStateController gameStateController;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        audioListener = GetComponent<AudioListener>();

        dialoguePosition = GetCameraTarget(GameObject.FindGameObjectWithTag("Player"));
        playerTransform = dialoguePosition.root;
    }

    void Start()
    {
        state = DialogueCameraState.Dormant;

        LevelManager.current.dialogueCamera = this;
        gameStateController = LevelManager.current.gameStateController;

        SetRendering(false);
    }

    void LateUpdate()
    {
        if (state != DialogueCameraState.Transitioning)
        {
            transform.position = dialoguePosition.position;
        }

        if (dialogueTarget)
        {
            Vector3 relative = dialogueTarget.position - playerTransform.position;
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            playerTransform.rotation = Quaternion.Euler(0f, angle, 0f);

            transform.LookAt(GetFocusPoint());
        }
    }

    private Vector3 GetFocusPoint()
    {
        return dialogueTarget.position + Vector3.up;
        //return ((dialogueTarget.position + playerTransform.position) / 2) + Vector3.up;
    }

    public Transform GetCameraTarget(GameObject character)
    {
        // Find gameobject tagged as the camera look target (ONLY searches one hierarchy level deep)
        foreach (Transform transform in character.transform)
        {
            if (transform.tag == "DialogueCameraTarget")
            {
                return transform;
            }
        }

        Debug.Log("Failed to get camera target.");
        return null;
    }

    public void SetRendering(bool rendering)
    {
        camera.enabled = rendering;
        audioListener.enabled = rendering;
    }

    // Smoothly transition to dialogue perspective
    public void Enter(Transform dialogueTarget, Transform startPosition)
    {
        gameStateController.SetPlayerFrozen(true);

        this.dialogueTarget = dialogueTarget;
        SetRendering(true);

        // Look transition camera to look at who we're talking to
        StartCoroutine(TransitionToDialogue(dialoguePosition, startPosition));
    }
    // Instantly pop into dialogue perspective
    public void InstantEnter(Transform dialogueTarget)
    {
        // freeze player control (and switching)
        gameStateController.SetPlayerFrozen(true);

        this.dialogueTarget = dialogueTarget;
        SetRendering(true);

        state = DialogueCameraState.Active;
    }

    public void StartExit(Transform mainCameraTransform)
    {
        StartCoroutine(BeginExit(mainCameraTransform));
    }

    private IEnumerator BeginExit(Transform mainCameraTransform)
    {
        

        // Transition camera back to our main camera
        yield return StartCoroutine(TransitionToMainCamera(mainCameraTransform));

        //yield return new WaitForSeconds(0.8f);
        
    }
    public void BeginInstantExit()
    {
        dialogueTarget = null;

        gameStateController.SetPlayerFrozen(false);
        Exit();
    }
    private void Exit()
    {
        SetRendering(false);
        state = DialogueCameraState.Dormant;

        LevelManager.current.mainCamera.SetRendering(true);

        Input.ResetInputAxes();
        // release player control (and switching)

        dialogueTarget = null;
        PlayerController.instance.TalkingPartner = null;
        gameStateController.SetPlayerFrozen(false);
    }

    private IEnumerator TransitionToDialogue(Transform newTarget, Transform startingTransform)
    {
        state = DialogueCameraState.Transitioning;

        transform.SetPositionAndRotation(startingTransform.position, startingTransform.rotation);

        Vector3 startingPosition = transform.position;

        Quaternion startingRotation = transform.rotation;


        //Interpolate target position to new target
        for (float t = 0; t < switchTime; t += Time.deltaTime * 1)
        {
            transform.position = Vector3.Lerp(startingPosition, newTarget.position, t / switchTime);
            transform.rotation = Quaternion.Lerp(startingRotation, Quaternion.LookRotation(GetFocusPoint() - (dialoguePosition.position), Vector3.up), t / switchTime);

            yield return null;
        }

        state = DialogueCameraState.Active;
    }
    private IEnumerator TransitionToMainCamera(Transform mainCameraTransform)
    {
        state = DialogueCameraState.Transitioning;

        Vector3 startingPosition = transform.position;
        Quaternion startingRotation = transform.rotation;

        //Interpolate target position to new target
        for (float t = 0; t < switchTime; t += Time.deltaTime * 1)
        {
            transform.position = Vector3.Lerp(startingPosition, mainCameraTransform.position, t / switchTime);
            transform.rotation = Quaternion.Lerp(startingRotation, mainCameraTransform.rotation, t / switchTime);

            yield return null;
        }

        Exit();
    }
}
