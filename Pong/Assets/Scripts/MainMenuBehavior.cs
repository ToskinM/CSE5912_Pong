using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public Button start, quit;
    void Start()
    {
        start.onClick.AddListener(PlayGame);
        quit.onClick.AddListener(QuitGame);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

}
