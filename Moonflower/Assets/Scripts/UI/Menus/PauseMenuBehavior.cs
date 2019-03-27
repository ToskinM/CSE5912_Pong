﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public GameObject Menu, OptionsMenu, ControlsMenu;
    public Button main, options, controls, quit, back1, back2;
    public ICommand returnMain, nag;

    private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.

    private AudioManager audioManager;
    public AudioSource MusicAudioSource;

    void Start()
    {
        main.onClick.AddListener(MainMenu);
        quit.onClick.AddListener(QuitGame);
        options.onClick.AddListener(Options);
        controls.onClick.AddListener(Controls);
        back1.onClick.AddListener(GoBack);
        back2.onClick.AddListener(GoBack);

        returnMain = new ReturnMenuCommand();
        nag = new NagCommand();
        //Music 
        //StartCoroutine(GetAudioManager());
        audioManager = AudioManager.instance;

    }

    private IEnumerator GetAudioManager()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            yield return null;
        }
    }

    public void MainMenu()
    {
        //returnMain.Execute();
        //GameStateController.current.ForceUnpause();
        SceneManager.UnloadSceneAsync(Constants.SCENE_PAUSEMENU);
        SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_MAINMENU);
        //FindObjectOfType<AudioManager>().Play("Menu");
    }

    public void QuitGame()
    {
        nag.Execute();
        //FindObjectOfType<AudioManager>().Play("Menu");
        Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

    public void GoBack()
    {
        Menu.SetActive(true);
        OptionsMenu.SetActive(false);
        ControlsMenu.SetActive(false); 
        //FindObjectOfType<AudioManager>().Play("Menu");
        //FindObjectOfType<AudioManager>().Pause("Collision");
    }

    public void Options()
    {
        Menu.SetActive(false);
        OptionsMenu.SetActive(true);
        ControlsMenu.SetActive(false);
        //FindObjectOfType<AudioManager>().Play("Menu");
    }

    public void Controls()
    {
        Menu.SetActive(false);
        ControlsMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        //FindObjectOfType<AudioManager>().Play("Menu");
    }

    public void Win()
    {
        //todo to display win
    }

    public void Lose()
    {
        //todo to display lose
    }


    void Update()
    {

    }
}




