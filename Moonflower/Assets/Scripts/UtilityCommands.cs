using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCommands : MonoBehaviour
{
    GameStateController gameStateController;

    ICommand screencapCmd, videocapCmd, pauseCmd, DebugViewCmd, QuickSaveCmd, QuickLoadCmd, PlayerKillCmd;

    void Start()
    {
        gameStateController = gameObject.GetComponent<GameStateController>();
        screencapCmd = new ScreenCaptureCommand();
        videocapCmd = new VideoCaptureCommand();
        pauseCmd = new PauseCommand();
        DebugViewCmd = new DebugViewCommand();
        QuickSaveCmd = new QuickSaveCommand();
        QuickLoadCmd = new QuickLoadCommand();
        PlayerKillCmd = new KillPlayerCommand();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseCmd.Execute();
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            //gameStateController.ToggleDebugMode();
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            //screencapCmd.Execute();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            //videocapCmd.Execute();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //DebugViewCmd.Execute();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //PlayerKillCmd.Execute();
        }
        //if (Input.GetKeyDown(KeyCode.F5))
        //{
        //    QuickSaveCmd.Execute();
        //}
        //if (Input.GetKeyDown(KeyCode.F9))
        //{
        //    QuickLoadCmd.Execute();
        //}
    }
}
