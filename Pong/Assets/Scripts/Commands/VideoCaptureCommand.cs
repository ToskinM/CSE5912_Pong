using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RockVR.Video;

public class VideoCaptureCommand : ICommand
{
    private bool isRecording;
    private GameStateManager gameStateManager;

    public VideoCaptureCommand()
    {
        isRecording = false;
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
    }

    public void Execute()
    {
        if (!isRecording)
        {
            isRecording = true;
            VideoCaptureCtrl.instance.StartCapture();

            Debug.Log("Started video capture.");
        }
        else
        {
            VideoCaptureCtrl.instance.StopCapture();
            isRecording = false;

            Debug.Log("Stopped video capture.");
        }
    }
}
