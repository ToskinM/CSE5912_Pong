using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayerCommand : ICommand
{
    public KillPlayerCommand()
    {
        
    }

    public void Execute()
    {
        GameObject.Find("Player").GetComponent<PlayerCombatController>().Kill();
    }

    public void Unexecute()
    {
        //not a thing
    }
}
