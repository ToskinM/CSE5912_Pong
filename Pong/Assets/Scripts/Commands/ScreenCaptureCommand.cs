using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureCommand : ICommand
{
    public GameStateManager gameStateManager;

    public ScreenCaptureCommand()
    {
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
    }

    public void Execute()
    {
        string dateTime = System.DateTime.Now.ToString("yyyyMMddhhMMss"); 
        string destinationPath = "Screenshots/";
        string fileName = "Screencap_" + dateTime + ".png";

        ScreenCapture.CaptureScreenshot(destinationPath + fileName);
        gameStateManager.DisplayScreencapText();

        Debug.Log("Screen capture saved.");
    }
}
