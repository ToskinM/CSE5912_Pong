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
            gameStateController.UnpauseGame();
        }
        else if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE)
        {
            SceneManager.LoadScene(Constants.SCENE_PAUSEMENU, LoadSceneMode.Additive);
            gameStateController.PauseGame();
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(Constants.SCENE_PAUSEMENU));
        }
    }

    public void Unexecute()
    {
        SceneManager.LoadScene(Constants.SCENE_PAUSEMENU, LoadSceneMode.Additive);
        gameStateController.TogglePause();
    }
}
