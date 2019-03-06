using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCommand : ICommand
{
    GameStateController gameStateController;

    public PauseCommand()
    {
        gameStateController = GameStateController.current;
    }

    public void Execute()
    {
        gameStateController = GameStateController.current;
        if (gameStateController.Paused)
        {
            SceneManager.UnloadSceneAsync(Constants.SCENE_PAUSEMENU);
            gameStateController.UnpauseGame();
        }
        else if (SceneManager.GetActiveScene().name != Constants.SCENE_MAINMENU)
        {
            SceneManager.LoadScene(Constants.SCENE_PAUSEMENU, LoadSceneMode.Additive);
            gameStateController.PauseGame();
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(Constants.SCENE_PAUSEMENU));
        }
    }

    public void Unexecute()
    {
        gameStateController = GameStateController.current;
        SceneManager.LoadScene(Constants.SCENE_PAUSEMENU, LoadSceneMode.Additive);
        gameStateController.TogglePause();
    }
}
