using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static GameStateController current;

    public GameObject Player;
    public GameObject MainCamera;

    public bool Paused;
    public bool DebugModeOn;

    private FollowCamera camControl;

    public delegate void Pause(bool isPaused);
    public static event Pause OnPaused;

    public delegate void FreezePlayer(bool freeze);
    public static event FreezePlayer OnFreezePlayer;

    private int menuLayers = 0;

    void Start()
    {
        if (current == null)
        {
            //DontDestroyOnLoad(gameObject);
            current = this;
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }

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
            menuLayers++;
            if (menuLayers > 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else          // Unlock
        {
            menuLayers--;
            if (menuLayers <= 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
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
        SetMouseLock(false);
    }
    public void UnpauseGame()
    {
        Paused = false;
        camControl.Frozen = false;
        OnPaused?.Invoke(Paused);
        SetMouseLock(true);
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
        PlayerMovementController playerMovement = Player.GetComponent<PlayerMovementController>();
        playerMovement.enabled = true;
    }

    public void SetPlayerFrozen(bool frozen)
    {
        OnFreezePlayer?.Invoke(frozen);
    }
}
