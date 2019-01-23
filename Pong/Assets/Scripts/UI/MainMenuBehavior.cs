using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public GameObject MainMenu, OptionsMenu; 
    public Button start, options, quit, back;
    public AudioClip MusicClip;
    public AudioSource MusicSource;
    public ICommand play, nag;

    private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.

    void Start()
    {
        //start.onClick.AddListener(PlayGame);
        
        start.onClick.AddListener(StartGame);
        options.onClick.AddListener(Options);
        quit.onClick.AddListener(QuitGame);
        back.onClick.AddListener(GoBack);

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

    public void GoBack()
    {
        MusicSource.Play();
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void Options()
    {
        MusicSource.Play();
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        //goOption.Execute();
    }

    public void QuitGame()
    {
        MusicSource.Play();
        nag.Execute();

        //Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

}
