using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public Button main, options, quit;
    public ICommand returnMain, goOptions, nag;

    //private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.

    void Start()
    {
        main.onClick.AddListener(MainMenu);
        quit.onClick.AddListener(QuitGame);
        options.onClick.AddListener(OptionsMenu);

        returnMain = new ReturnMenuCommand();
        nag = new NagCommand();
        goOptions = new OptionsCommand();
    }

    public void MainMenu()
    {
        returnMain.Execute();
    }

    public void QuitGame()
    {
        nag.Execute();

        //Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

    public void OptionsMenu()
    {
        goOptions.Execute();
    }

    public void Win()
    {
        //todo to display win
    }

    public void Lose()
    {
        //todo to display lose
    }


}




