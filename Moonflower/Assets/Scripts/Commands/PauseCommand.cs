using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCommand : ICommand
{
    GameStateController gameStateController;

    public PauseCommand()
    {
        gameStateController = GameObject.Find("Game State Manager").GetComponent<GameStateController>();
    }

    public void Execute()
    {
        if (gameStateController.Paused)
        {
            SceneManager.UnloadSceneAsync(Constants.SCENE_PAUSEMENU);
            gameStateController.TogglePause();
        }
        else if (SceneManager.GetActiveScene().name == Constants.SCENE_GAME)
        {
            SceneManager.LoadScene(Constants.SCENE_PAUSEMENU, LoadSceneMode.Additive);
            gameStateController.TogglePause();
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(Constants.SCENE_PAUSEMENU));
        }
    }
}
