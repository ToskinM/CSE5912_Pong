using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCommands : MonoBehaviour
{
    GameStateController gameStateController;

    ICommand screencapCmd, videocapCmd, pauseCmd;

    // Start is called before the first frame update
    void Start()
    {
        gameStateController = gameObject.GetComponent<GameStateController>();
        screencapCmd = new ScreenCaptureCommand();
        videocapCmd = new VideoCaptureCommand();
        pauseCmd = new PauseCommand();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCmd.Execute();
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            gameStateController.ToggleDebugMode();
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            screencapCmd.Execute();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            videocapCmd.Execute();
        }
    }
}
