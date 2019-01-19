using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureCommand : ICommand
{
    public void Execute()
    {
        string dateTime = System.DateTime.Now.ToString("yyyyMMddhhMMss"); // Get system time to avoid writing over the same file
        ScreenCapture.CaptureScreenshot("Screenshots/Screencap_" + dateTime + ".png"); // Save to Screenshots folder in root folder

        Debug.Log("Screen capture saved.");
    }
}
