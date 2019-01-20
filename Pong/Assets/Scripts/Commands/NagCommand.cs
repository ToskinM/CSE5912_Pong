using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NagCommand : ICommand
{
    public void Execute()
    {
        SceneManager.LoadScene("Quit Nag Popup");
    }
}
