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

    public delegate void FreezePlayer(bool freeze);
    public static event FreezePlayer OnFreezePlayer;

    void Start()
    {
        Paused = false;
        DebugModeOn = false;
        camControl = MainCamera.GetComponent<FollowCamera>();

        // Lock Mouse on game start
        SetMouseLock(true);
    }

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

    public void SetMouseLock(bool doLock)
    {
        if (doLock)   // Lock
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else          // Unlock
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void TogglePause()
    {
        if (Paused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
        
        OnPaused?.Invoke(Paused);

        if (camControl.Frozen)
            UnfreezeCamera();
        else
            FreezeCamera(); 
    }

    public void PauseGame()
    {
        Paused = true;
        camControl.Frozen = true;
        OnPaused?.Invoke(Paused);
    }
    public void UnpauseGame()
    {
        Paused = false;
        camControl.Frozen = false;
        OnPaused?.Invoke(Paused);
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

    public void SetPlayerFrozen(bool frozen)
    {
        OnFreezePlayer?.Invoke(frozen);
    }
}
