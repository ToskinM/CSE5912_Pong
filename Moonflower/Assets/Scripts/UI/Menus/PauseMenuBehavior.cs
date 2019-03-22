using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBehavior : MonoBehaviour
{
    public GameObject Menu, OptionsMenu;
    public Button main, options, quit, back;
    public ICommand returnMain, nag;
    public Slider musicSlider;
    public Slider audioSlider;

    private float originalMusicVol;
    private float originalAudioVol;
    private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.

    private AudioManager audioManager;

    void Start()
    {
        main.onClick.AddListener(MainMenu);
        quit.onClick.AddListener(QuitGame);
        options.onClick.AddListener(Options);
        back.onClick.AddListener(GoBack);

        returnMain = new ReturnMenuCommand();
        nag = new NagCommand();
        //Music 
        //StartCoroutine(GetAudioManager());
        audioManager = AudioManager.instance;
        musicSlider.value = GetBackgroundVolume();
        audioSlider.value = GetSoundVolume();

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
        //FindObjectOfType<AudioManager>().Play("Menu");
        //FindObjectOfType<AudioManager>().Pause("Collision");
    }

    public void Options()
    {
        Menu.SetActive(false);
        OptionsMenu.SetActive(true);
        //FindObjectOfType<AudioManager>().Play("Menu");
        //slider.value = FindObjectOfType<AudioManager>().GetVolume("Backgroud");
        originalMusicVol = musicSlider.value;
        originalAudioVol = audioSlider.value;
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
        if (Mathf.Abs(musicSlider.value - originalMusicVol) >= 0.01)
        {
            ChangeBackgroundVol(musicSlider.value);
        }
        if (Mathf.Abs(audioSlider.value - originalMusicVol) >= 0.01)
        {
            ChangeAudioVol(audioSlider.value);
        }

    }
    public float GetBackgroundVolume()
    {
        return audioManager.GetBackgroundVolume();
    }
    public float GetSoundVolume()
    {
        return audioManager.GetSoundVolume();
    }
    public void ChangeBackgroundVol(float vol)
    {
        audioManager.ChangeBackgroundVol(vol);
    }
    public void ChangeAudioVol(float vol)
    {
        audioManager.ChangeSoundVol(vol);
    }

    void Update()
    {
        //if (OptionsMenu.activeInHierarchy == true)
        //    Slider();
    }
}




