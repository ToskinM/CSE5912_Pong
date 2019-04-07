using System.Collections;
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

    public MainMenuCamera mainMenuCamera;

    private bool fading = false;
    private const float pseudoDeltaTime = 0.05f;
    private readonly string MenuSFX = "Menu";

    //private AudioManager audioManager;
   
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
        audioManager.Play(MenuSFX, "Click");
        sceneController = FindObjectOfType<SceneController>();
        if (sceneController != null)
        {
            sceneController.FadeAndLoadScene(Constants.SCENE_ANAIHOUSE);
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
        SceneController.current.FadeAndLoadScene(Constants.SCENE_ANAIHOUSE);
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
        audioManager.Play(MenuSFX, "Click");
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
        audioManager.Play(MenuSFX, "Click");
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
        audioManager.Play(MenuSFX, "Quit");
        nag.Execute();

        if (mainMenuCamera)
        {
            mainMenuCamera.SetPosition(2);
        }

        //Application.Quit(); // this doesn't affect the unity editor, only a built application
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
    }

    private IEnumerator FadeTitle(float finalAlpha, float duration)
    {
        fading = true;

        float fadeSpeed = Mathf.Abs(Title.alpha - finalAlpha) / duration;
        while (!Mathf.Approximately(Title.alpha, finalAlpha))
        {
            //Debug.Log("k");
            Title.alpha = Mathf.MoveTowards(Title.alpha, finalAlpha, fadeSpeed * pseudoDeltaTime);
            yield return null;
        }

        fading = false;
    }
}
