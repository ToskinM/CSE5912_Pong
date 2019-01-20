using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillNagCommand : ICommand
{
    public void Execute()
    {
        SceneManager.UnloadSceneAsync("Quit Nag Popup");
    }
}
