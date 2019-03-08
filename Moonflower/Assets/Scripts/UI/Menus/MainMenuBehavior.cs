﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public GameObject MainMenu, OptionsMenu;
    public CanvasGroup Title;
    public Button start, options, quit, back;
    //public AudioClip MusicClip;
    //public AudioSource MusicSource;
    public ICommand play, nag;
    public Slider musicSlider;
    public Slider audioSlider;

    public MainMenuCamera mainMenuCamera;

    private bool fading = false;
    private const float pseudoDeltaTime = 0.05f;

    //private AudioManager audioManager;
    private float originalMusicVol;
    private float originalAudioVol;
    private SceneController sceneController;    // Reference to the SceneController to actually do the loading and unloading of scenes.
    AudioManager audioManager;

    void Awake()
    {
        //StartCoroutine(GetAudioManager());
    }

    void Start()
    {
        start.onClick.AddListener(StartGame);
        options.onClick.AddListener(Options);
        quit.onClick.AddListener(QuitGame);
        back.onClick.AddListener(GoBack);

        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager)
        {
            musicSlider.value = audioManager.GetBackgroundVolume();
            audioSlider.value = audioManager.GetSoundVolume();

            originalMusicVol = musicSlider.value;
            originalAudioVol = audioSlider.value;
        }

        play = new PlayCommand();
        nag = new NagCommand();

        //GameStateController.current.SetMouseLock(false);
        GameStateController.current.ForceMouseUnlock();

        //MainMenu.SetActive(false);
        Title.alpha = 0f;

        //StartCoroutine(FadeTitle(1f, 2f));
    }
    public void StartGame()
    {
        //FindObjectOfType<AudioManager>().Play("Menu");
        sceneController = FindObjectOfType<SceneController>();
        if (sceneController != null)
        {
            sceneController.FadeAndLoadScene(Constants.SCENE_VILLAGE);
        }
        else
        {
            Invoke("DelayMethod", 1f);
        }
    }
    void DelayMethod()
    {
        //if no delay sound won't play for "start"
        //play.Execute();
        SceneController.current.FadeAndLoadScene(Constants.SCENE_VILLAGE);
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
        //FindObjectOfType<AudioManager>().Play("Menu");
        //Title.SetActive(true);
        //MainMenu.SetActive(true);
        //OptionsMenu.SetActive(false);

        if (mainMenuCamera)
        {
            mainMenuCamera.SetPosition(0);
        }
    }

    public void Options()
    {
        //FindObjectOfType<AudioManager>().Play("Menu");
        //Title.SetActive(false);
        //MainMenu.SetActive(false);
        //OptionsMenu.SetActive(true);
        //goOption.Execute();

        if (mainMenuCamera)
        {
            mainMenuCamera.SetPosition(1);
        }
    }

    public void QuitGame()
    {
        //FindObjectOfType<AudioManager>().Play("Menu");
        nag.Execute();

        if (mainMenuCamera)
        {
            mainMenuCamera.SetPosition(2);
        }

        //Application.Quit(); // this doesn't affect the unity editor, only a built application
    }
    public void Slider()
    {

        if (Mathf.Abs(musicSlider.value-originalMusicVol)>=0.01)
        {
            if (audioManager)
            {
                audioManager.ChangeBackgroundVol(musicSlider.value);
            }

            //FindObjectOfType<AudioManager>().PlayTest("Background");
        }
    }

    //private IEnumerator GetAudioManager()
    //{
    //    while (audioManager == null)
    //    {
    //        audioManager = FindObjectOfType<AudioManager>();
    //        yield return null;
    //    }
    //    audioManager.Play("Background");

    //}

    void Update()
    {
        if (OptionsMenu.activeInHierarchy == true)
            Slider();
    }

    private IEnumerator FadeTitle(float finalAlpha, float duration)
    {
        fading = true;

        float fadeSpeed = Mathf.Abs(Title.alpha - finalAlpha) / duration;
        while (!Mathf.Approximately(Title.alpha, finalAlpha))
        {
            Debug.Log("k");
            Title.alpha = Mathf.MoveTowards(Title.alpha, finalAlpha, fadeSpeed * pseudoDeltaTime);
            yield return null;
        }

        fading = false;
    }
}
