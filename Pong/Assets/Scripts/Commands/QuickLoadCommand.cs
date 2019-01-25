using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickLoadCommand : ICommand
{
    GameStateManager gameStateManager;

    public QuickLoadCommand()
    {
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
    }

    public void Execute()
    {
        if (!gameStateManager.Paused && SceneManager.GetActiveScene().name == Constants.SCENE_PONG)
        {
            gameStateManager.LoadState();
        }
    }
}