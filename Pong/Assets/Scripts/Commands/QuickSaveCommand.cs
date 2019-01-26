using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSaveCommand : ICommand
{
    GameStateManager gameStateManager;

    public QuickSaveCommand()
    {
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
    }

    public void Execute()
    {
        if (!gameStateManager.Paused && SceneManager.GetActiveScene().name == Constants.SCENE_PONG)
        {
            gameStateManager.SaveState();
        }
    }
}