using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public GameObject Menu, OptionsMenu;
    public Button main, options, quit, back;
    public ICommand returnMain, nag;

    //private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.

    void Start()
    {
        main.onClick.AddListener(MainMenu);
        quit.onClick.AddListener(QuitGame);
        options.onClick.AddListener(Options);
        back.onClick.AddListener(GoBack);

        returnMain = new ReturnMenuCommand();
        nag = new NagCommand();
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

    public void GoBack()
    {
        Menu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void Options()
    {
        Menu.SetActive(false);
        OptionsMenu.SetActive(true);
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




