﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillNagCommand : ICommand
{
    public void Execute()
    {
        //if (SceneManager.GetActiveScene().name == "PongTest")
        //{
        //    new PauseCommand().Execute();
        //}

        SceneManager.UnloadSceneAsync(Constants.SCENE_QUITPOPUP);
    }
}
