using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public GameObject Menu, OptionsMenu;
    public Button main, options, quit, back;
    public ICommand returnMain, nag;
    public Slider slider;

    private float originalVolume;
    //private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.

    void Start()
    {
        main.onClick.AddListener(MainMenu);
        quit.onClick.AddListener(QuitGame);
        options.onClick.AddListener(Options);
        back.onClick.AddListener(GoBack);

        returnMain = new ReturnMenuCommand();
        nag = new NagCommand();
        //slider.value = FindObjectOfType<AudioManager>().GetVolume("Backgroud");
        originalVolume = slider.value;
    }

    public void MainMenu()
    {
        returnMain.Execute();
        FindObjectOfType<AudioManager>().Play("Menu");
    }

    public void QuitGame()
    {
        nag.Execute();
        FindObjectOfType<AudioManager>().Play("Menu");
        //Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

    public void GoBack()
    {
        Menu.SetActive(true);
        OptionsMenu.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Menu");
        FindObjectOfType<AudioManager>().Pause("Collision");
    }

    public void Options()
    {
        Menu.SetActive(false);
        OptionsMenu.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Menu");
        slider.value = FindObjectOfType<AudioManager>().GetVolume("Backgroud");
        originalVolume = slider.value;
    }

    public void Win()
    {
        //todo to display win
    }

    public void Lose()
    {
        //todo to display lose
    }

    public void Slider()
    {
        FindObjectOfType<AudioManager>().ChangeVolume("Background", slider.value);
        //if (Mathf.Abs(slider.value-originalVolume)>=0.01)
        //{
        //    FindObjectOfType<AudioManager>().PlayTest("Background");
        //}
    }
    void Update()
    {
        if (OptionsMenu.activeInHierarchy == true)
            Slider();


    }
}




