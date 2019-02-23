using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public GameObject Player;
    public GameObject MainCamera;

    public bool Paused;
    public bool DebugModeOn;

    private FollowCamera camControl;

    public delegate void Pause(bool isPaused);
    public static event Pause OnPaused;

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
        DebugModeOn = false;
        camControl = MainCamera.GetComponent<FollowCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        camControl = MainCamera.GetComponent<FollowCamera>();
    }

    public void ToggleDebugMode()
    {
        DebugModeOn = !DebugModeOn;

        //CameraController camControl = MainCamera.GetComponent<CameraController>();
        camControl = MainCamera.GetComponent<FollowCamera>();

        camControl.SetFreeRoam(DebugModeOn);
        EnablePlayerMovement(!DebugModeOn);
    }


    public void TogglePause()
    {
        Paused = !Paused;
        OnPaused?.Invoke(Paused);

        if (camControl.Frozen)
            UnfreezeCamera();
        else
            FreezeCamera(); 

    }

    public void FreezeCamera()
    {
       camControl.Frozen = true; 
    }

    public void UnfreezeCamera()
    {
        camControl.Frozen = false;
    }

    private void EnablePlayerMovement(bool enabled)
    {
        PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
    }
}
