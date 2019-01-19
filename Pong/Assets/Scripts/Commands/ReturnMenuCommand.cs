using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMenuCommand : ICommand
{
    public void Execute()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
