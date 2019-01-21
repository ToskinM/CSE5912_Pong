using System.Collections;
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
    void Start()
    {
        MusicSource.clip = MusicClip;
        start.onClick.AddListener(PlayGame);
        quit.onClick.AddListener(QuitGame);
        play = new PlayCommand();
        nag = new NagCommand(); 
    }
    public void PlayGame()
    {
        MusicSource.Play();
        Invoke("DelayMethod", 1f);
    }
    void DelayMethod()
    {
        //if no delay sound won't play for "start"
        play.Execute();
    }
    public void QuitGame()
    {
        MusicSource.Play();
        nag.Execute();

        //Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

}
