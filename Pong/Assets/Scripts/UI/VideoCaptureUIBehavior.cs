using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RockVR.Video;
using System.Diagnostics;

public class VideoCaptureUIBehavior : MonoBehaviour
{
    private float elapsedTime;

    private void Awake()
    {
        Application.runInBackground = true;
    }

    private void Start()
    {
        elapsedTime = 0f;
    }

    private void OnGUI()
    {
        if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED)
        {
            Application.runInBackground = true;

            elapsedTime += Time.deltaTime;
            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)elapsedTime;
            GUI.Label(new Rect(10, 20, 150, 50), "Recording: " + minutes.ToString() + "m " + seconds.ToString() + "s");
        }
        else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STOPPED)
        {
            GUI.Label(new Rect(10, 20, 150, 50), "Saving..");
            elapsedTime = 0f;
        }
    }
}
