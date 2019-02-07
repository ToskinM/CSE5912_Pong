using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreezeCameraCommand : ICommand
{
    GameStateController gameStateController;

    public FreezeCameraCommand()
    {
        gameStateController = GameObject.Find("Game State Manager").GetComponent<GameStateController>();
    }

    public void Execute()
    {
        //if (gameStateController.Paused)
        //{
        //    SceneManager.UnloadSceneAsync(Constants.SCENE_PAUSEMENU);
        //    gameStateController.TogglePause();
        //}
        //if (SceneManager.GetActiveScene().name == Constants.SCENE_GAME)
        {
            //SceneManager.LoadScene(Constants.SCENE_PAUSEMENU, LoadSceneMode.Additive);
            gameStateController.FreezeCamera();
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(Constants.SCENE_PAUSEMENU));
        }
    }
}
