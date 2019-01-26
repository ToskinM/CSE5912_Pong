using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPongCommand : ICommand
{
    GameStateManager gameStateManager;

    public ResetPongCommand()
    {
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
    }

    public void Execute()
    {
        if (!gameStateManager.Paused && SceneManager.GetActiveScene().name == Constants.SCENE_PONG)
        {
            gameStateManager.ResetGame();
        }
    }
}