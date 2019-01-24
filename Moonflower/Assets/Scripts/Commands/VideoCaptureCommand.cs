using System.Collections;
using System.Collections.Generic;
using RockVR.Video;
using UnityEngine;

public class VideoCaptureCommand : ICommand
{
    private bool isRecording;

    public VideoCaptureCommand()
    {
        isRecording = false;
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
