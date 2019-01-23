using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCommand : ICommand
{
    GameStateManager gameStateManager;

    public PauseCommand()
    {
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
    }

    public void Execute()
    {

        if (gameStateManager.Paused)
        {
            SceneManager.UnloadSceneAsync("Pause Menu");
            gameStateManager.TogglePause();
        }
        else if (SceneManager.GetActiveScene().name == "PongTest")
        {
            SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
            gameStateManager.TogglePause();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Pause Menu"));
        }
    }
}
