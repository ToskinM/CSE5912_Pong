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
            SceneManager.UnloadSceneAsync("Pause Menu");
            gameStateController.TogglePause();
        }
        else if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
            gameStateController.TogglePause();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Pause Menu"));
        }
    }
}
