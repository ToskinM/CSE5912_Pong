﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{

    public Button start, quit;
    public AudioClip MusicClip;
    public AudioSource MusicSource;
    public ICommand play, nag;

    private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.

    void Start()
    {
        //start.onClick.AddListener(PlayGame);
        
        start.onClick.AddListener(StartGame);

        quit.onClick.AddListener(QuitGame);
        MusicSource.clip = MusicClip;
        play = new PlayCommand();
        nag = new NagCommand();
    }
    public void PlayGame()
    {
        MusicSource.Play();
        Invoke("DelayMethod", 1f);
    }
    public void StartGame()
    {
        MusicSource.Play();
        sceneController = FindObjectOfType<SceneController>();
        if (sceneController != null)
        {
            sceneController.FadeAndLoadScene(Constants.SCENE_PONG);
        }
        else
        {
            Invoke("DelayMethod", 1f);
        }
    }
    void DelayMethod()
    {
        //if no delay sound won't play for "start"
        play.Execute();
    }

    public void Win()
    {
        //todo to display win
    }

    public void Lose()
    {
        //todo to display lose
    }

    public void QuitGame()
    {
        MusicSource.Play();
        nag.Execute();

        //Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

}
