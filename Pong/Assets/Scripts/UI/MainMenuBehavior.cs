using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public Button start, quit;
    public ICommand play, nag; 
    void Start()
    {
        start.onClick.AddListener(PlayGame);
        quit.onClick.AddListener(QuitGame);

        play = new PlayCommand();
        nag = new NagCommand(); 
    }
    public void PlayGame()
    {
        play.Execute(); 
    }

    public void QuitGame()
    {
        nag.Execute();
        //Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

}
