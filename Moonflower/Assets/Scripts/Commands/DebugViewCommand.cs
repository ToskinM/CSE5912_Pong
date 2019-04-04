using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugViewCommand : ICommand
{
    Camera camera;

    public DebugViewCommand()
    {
        //camera = LevelManager.current.mainCamera.gameObject.GetComponent<Camera>();
    }

    public void Execute()
    {
        LevelManager.current.mainCamera.gameObject.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Debug");

        GameStateController.current.ToggleDebugView();
    }

    public void Unexecute()
    {
        //not a thing
    }
}
