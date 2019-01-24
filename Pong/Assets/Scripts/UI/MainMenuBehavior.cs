using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public GameObject MainMenu, OptionsMenu; 
    public Button start, options, quit, back;
    //public AudioClip MusicClip;
    //public AudioSource MusicSource;
    public ICommand play, nag;

    private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.

    void Start()
    {
        start.onClick.AddListener(StartGame);
        options.onClick.AddListener(Options);
        quit.onClick.AddListener(QuitGame);
        back.onClick.AddListener(GoBack);

        play = new PlayCommand();
        nag = new NagCommand();
    }
    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
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
        FindObjectOfType<AudioManager>().Play("Menu");
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void Options()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        //goOption.Execute();
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        nag.Execute();

        //Application.Quit(); // this doesn't affect the unity editor, only a built application
    }
 
}
