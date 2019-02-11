using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NagCommand : ICommand
{
    public void Execute()
    {
        //if (SceneManager.GetActiveScene().name == "PongTest")
        //{
        //    new PauseCommand().Execute();
        //}

        SceneManager.LoadScene("Quit Nag Popup",LoadSceneMode.Additive);
        
    }

    public void Unexecute()
    {
        SceneManager.UnloadSceneAsync(Constants.SCENE_QUITPOPUP);
    }
}
