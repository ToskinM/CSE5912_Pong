using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupBehavior : MonoBehaviour
{
    public Button yes, no;
    //public AudioClip MusicClip;
    //public AudioSource MusicSource;
    ICommand cont; 
    void Start()
    {
        yes.onClick.AddListener(Quit);
        no.onClick.AddListener(Continue);
        //MusicSource.clip = MusicClip;
        cont = new KillNagCommand();
    }

    public void Quit()
    {
        //MusicSource.Play();
        //Debug.Log("Quit");
        Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

    public void Continue()
    {
        //MusicSource.Play();
        //SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
        cont.Execute();
    }

}
