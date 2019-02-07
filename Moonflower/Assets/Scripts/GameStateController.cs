using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public GameObject Player;
    public GameObject MainCamera;

    public bool Paused;
    public bool DebugModeOn;

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
        DebugModeOn = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleDebugMode()
    {
        DebugModeOn = !DebugModeOn;

        //CameraController camControl = MainCamera.GetComponent<CameraController>();
        FollowCamera camControl = MainCamera.GetComponent<FollowCamera>();

        camControl.SetFreeRoam(DebugModeOn);
        EnablePlayerMovement(!DebugModeOn);
    }


    public void TogglePause()
    {
        Paused = !Paused;

        EnablePlayerMovement(!Paused);
        FreezeCamera(); 

    }

    public void FreezeCamera()
    {
        MainCamera.GetComponent<FollowCamera>().ToggleFreeze(); 
    }

    private void EnablePlayerMovement(bool enabled)
    {
        PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();
        playerMovement.enabled = enabled;
    }
}
