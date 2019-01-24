using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureCommand : ICommand
{
    private ScreenCaptureUI screencapUI;

    public ScreenCaptureCommand()
    {
        screencapUI = GameObject.Find("UI Manager").GetComponent<ScreenCaptureUI>();
    }

    public void Execute()
    {
        string dateTime = System.DateTime.Now.ToString("yyyyMMddhhMMss");
        string destinationPath = "Screenshots/";
        string fileName = "Screencap_" + dateTime + ".png";

        ScreenCapture.CaptureScreenshot(destinationPath + fileName);
        screencapUI.DisplayScreencapText();

        Debug.Log("Screen capture saved.");
    }
}
