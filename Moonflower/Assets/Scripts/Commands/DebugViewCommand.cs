using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugViewCommand : ICommand
{
    Camera camera;

    public DebugViewCommand()
    {
        camera = Camera.main;
    }

    public void Execute()
    {
        camera.cullingMask ^= 1 << LayerMask.NameToLayer("Debug");
    }

    public void Unexecute()
    {
        //not a thing
    }
}
