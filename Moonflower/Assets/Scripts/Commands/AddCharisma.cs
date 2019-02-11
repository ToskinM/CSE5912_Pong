using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddCharisma : ICommand
{
    GameStateController gameStateController;

    public AddCharisma()
    {
        gameStateController = GameObject.Find("Player").GetComponent<GameStateController>();
    }

    public void Execute()
    {
    }

    public void Unexecute()
    {
        //not a thing
    }
}
