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
        gameStateManager.Pause();
    }
}
