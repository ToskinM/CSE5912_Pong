using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// Unity Tutorial Script
public class SceneController : MonoBehaviour
{
    public static SceneController current;

    public event Action BeforeSceneUnload;
    public event Action AfterSceneLoad;

    public string sceneDiedAt;

    [Header("Fade To/From Black")]
    public CanvasGroup faderCanvasGroupBlack;
    public CanvasGroup faderCanvasGroupWhite;
    public float fadeDuration = 1f;
    private bool isFading;
    public bool isLoading;

    [Header("Loadscreen")]
    public GameObject loadscreen;
    public CanvasGroup loadscreenCanvasGroup;
    public Slider loadingBar;
    public TextMeshProUGUI progressText;

    public List<string> WentScene;

    private void Start()
    {
        // Only allow one scene controller at a time
        if (current == null)
        {
            DontDestroyOnLoad(gameObject);
            current = this;
        }
        else if (current != this)
        {
            Destroy(gameObject);
        }

        loadscreen.SetActive(false);
        faderCanvasGroupBlack.alpha = 1f;
        faderCanvasGroupWhite.alpha = 0f;
        loadscreenCanvasGroup.alpha = 0f;
        StartCoroutine(Fade(0f));
    }

    // Call these to switch scenes (NOT ADDITIVE)
    //
    public void FadeAndLoadScene(String sceneName)
    {
        if (!isFading && !isLoading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }
    // (without sexy loadscreen)
    public void FadeAndLoadSceneNoLS(String sceneName)
    {
        if (!isFading && !isLoading)
        {
            StartCoroutine(FadeAndSwitchScenesNoLS(sceneName));

        }
    }
    public void FadeAndLoadSceneGameOver(String sceneName)
    {
        if (!isFading && !isLoading)
        {
            StartCoroutine(FadeAndSwitchScenesGameOver(sceneName));
        }
    }
    public void FadeAndLoadSceneVision(String sceneName)
    {
        if (!isFading && !isLoading)
        {
            StartCoroutine(FadeAndSwitchScenesVision(sceneName));
        }
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        WentedScene();
        isLoading = true;

        // Fade to black
        yield return StartCoroutine(Fade(1f));
        BeforeSceneUnload?.Invoke();
        GameStateController.current?.ForceUnpause();
        HideSingletons();

        // load loading scene
        yield return SceneManager.LoadSceneAsync(Constants.SCENE_LOADING, LoadSceneMode.Additive);

        // Unload previous scene
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // Unload loading scene
        yield return SceneManager.UnloadSceneAsync(Constants.SCENE_LOADING);
        AfterSceneLoad?.Invoke();
        isLoading = false;
        ShowSingletons();
        if (sceneName != Constants.SCENE_MAINMENU) PlayerController.instance.SpawnPlayerObjects();

        if (sceneName == Constants.SCENE_CAVE || sceneName == Constants.SCENE_CAVEBOSS)
        {
            PlayerController.instance.EnableSwitching();
        }

        // Fade to new scene
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator FadeAndSwitchScenesNoLS(string sceneName)
    {
        WentedScene();
        isLoading = true;

        // Fade to black
        yield return StartCoroutine(Fade(1f));
        BeforeSceneUnload?.Invoke();
        GameStateController.current?.ForceUnpause();
        HideSingletons();

        // load loading scene
        yield return SceneManager.LoadSceneAsync(Constants.SCENE_LOADING, LoadSceneMode.Additive);

        // Unload previous scene (without loadscreen)
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActiveNoLS(sceneName));

        // Unload loading scene
        yield return SceneManager.UnloadSceneAsync(Constants.SCENE_LOADING);

        AfterSceneLoad?.Invoke();
        isLoading = false;
        ShowSingletons();
        if (PlayerController.instance != null && sceneName != Constants.SCENE_MAINMENU) PlayerController.instance.SpawnPlayerObjects();

        if (sceneName == Constants.SCENE_CAVE || sceneName == Constants.SCENE_CAVEBOSS)
        {
            PlayerController.instance.EnableSwitching();
        }

        // Fade to new scene
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator FadeAndSwitchScenesGameOver(string sceneName)
    {
        isLoading = true;
        sceneDiedAt = SceneManager.GetActiveScene().name;

        // Fade to black
        yield return StartCoroutine(Fade(1f, 5f));
        BeforeSceneUnload?.Invoke();
        GameStateController.current?.ForceUnpause();

        // load loading scene
        yield return SceneManager.LoadSceneAsync(Constants.SCENE_LOADING, LoadSceneMode.Additive);

        // Unload previous scene (without loadscreen)
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        DestroySingletons();
        yield return StartCoroutine(LoadSceneAndSetActiveNoLS(sceneName));

        // Unload loading scene
        yield return SceneManager.UnloadSceneAsync(Constants.SCENE_LOADING);
        AfterSceneLoad?.Invoke();
        isLoading = false;


        // Fade to new scene
        yield return StartCoroutine(Fade(0f, 0.5f));
    }
    private IEnumerator FadeAndSwitchScenesVision(string sceneName)
    {
        isLoading = true;
        sceneDiedAt = SceneManager.GetActiveScene().name;

        // Fade to black
        yield return StartCoroutine(FadeWhite(1f, 5f));
        BeforeSceneUnload?.Invoke();
        GameStateController.current?.ForceUnpause();

        // load loading scene
        yield return SceneManager.LoadSceneAsync(Constants.SCENE_LOADING, LoadSceneMode.Additive);

        // Unload previous scene (without loadscreen)
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        DestroySingletons();
        yield return StartCoroutine(LoadSceneAndSetActiveNoLS(sceneName));

        // Unload loading scene
        yield return SceneManager.UnloadSceneAsync(Constants.SCENE_LOADING);
        AfterSceneLoad?.Invoke();
        isLoading = false;


        // Fade to new scene
        yield return StartCoroutine(FadeWhite(0f, 0.5f));
    }

    public void WentedScene()
    {
        if (!WentScene.Contains(SceneManager.GetActiveScene().name))
            WentScene.Add(SceneManager.GetActiveScene().name);
        else
        {
//            Debug.Log("I went here before");
        }
        for (int i = 0; i < WentScene.Count;i++)
        {
//            Debug.Log(WentScene[i].ToString());
        }
    }


    public void FadeOutToBlack(float multiplier = 1)
    {
        StartCoroutine(FadeToBlack(multiplier));
    }
    public void FadeInFromBlack(float multiplier = 1)
    {
        StartCoroutine(FadeFromBlack(multiplier));
    }
    public IEnumerator FadeToBlack(float multiplier = 1)
    {
        yield return StartCoroutine(Fade(1f, multiplier));
    }
    public IEnumerator FadeFromBlack(float multiplier = 1)
    {
        yield return StartCoroutine(Fade(0f, multiplier));
    }

    // Load coroutines
    //
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        loadscreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingBackground(1f));
        loadingBar.value = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            progressText.text = (asyncLoad.progress * 100).ToString("F0") + "%";
            yield return null;
        }
        StartCoroutine(FadeLoadingBackground(0f));
        loadscreen.SetActive(false);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }
    private IEnumerator LoadSceneAndSetActiveNoLS(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    // Fade Coroutines
    //
    private IEnumerator Fade(float finalAlpha, float speedMultiplier = 1)
    {
        isFading = true;
        faderCanvasGroupBlack.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(faderCanvasGroupBlack.alpha - finalAlpha) / (fadeDuration * speedMultiplier);
        while (!Mathf.Approximately(faderCanvasGroupBlack.alpha, finalAlpha))
        {
            faderCanvasGroupBlack.alpha = Mathf.MoveTowards(faderCanvasGroupBlack.alpha, finalAlpha, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
        isFading = false;
        faderCanvasGroupBlack.blocksRaycasts = false;
    }
    private IEnumerator FadeWhite(float finalAlpha, float speedMultiplier = 1)
    {
        isFading = true;
        faderCanvasGroupWhite.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(faderCanvasGroupWhite.alpha - finalAlpha) / (fadeDuration * speedMultiplier);
        while (!Mathf.Approximately(faderCanvasGroupWhite.alpha, finalAlpha))
        {
            faderCanvasGroupWhite.alpha = Mathf.MoveTowards(faderCanvasGroupWhite.alpha, finalAlpha, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
        isFading = false;
        faderCanvasGroupWhite.blocksRaycasts = false;
    }

    private IEnumerator FadeLoadingBackground(float finalAlpha)
    {
        isFading = true;
        loadscreenCanvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(loadscreenCanvasGroup.alpha - finalAlpha) / (fadeDuration / 3);
        while (!Mathf.Approximately(loadscreenCanvasGroup.alpha, finalAlpha))
        {
            loadscreenCanvasGroup.alpha = Mathf.MoveTowards(loadscreenCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
        isFading = false;
        loadscreenCanvasGroup.blocksRaycasts = false;
    }

    private void HideSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(false);
        if (UISingleton.instance != null) UISingleton.instance.GetComponent<CanvasGroup>().alpha = 0;
 //       Debug.Log("Hide");
    }

    private void ShowSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(true);
        if (UISingleton.instance != null) UISingleton.instance.GetComponent<CanvasGroup>().alpha = 1;
//        Debug.Log("Show");
    }

    public void DestroySingletons()
    {
        Destroy(PlayerController.instance.GetActivePlayerObject());
        Destroy(PlayerController.instance.GetCompanionObject());
        Destroy(PlayerController.instance.gameObject);
        Destroy(SpawnPoint.current.gameObject);
        Destroy(UISingleton.instance.gameObject);
        Destroy(GameStateController.current.gameObject);
        Destroy(AudioManager.instance.gameObject);
    }

    public IEnumerator BlinkIn(float speed)
    {
        isFading = true;
        faderCanvasGroupBlack.blocksRaycasts = true;

        // Fade to black
        float fadeSpeed = Mathf.Abs(faderCanvasGroupBlack.alpha - 1) / (speed);
        while (!Mathf.Approximately(faderCanvasGroupBlack.alpha, 1))
        {
            faderCanvasGroupBlack.alpha = Mathf.MoveTowards(faderCanvasGroupBlack.alpha, 1, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        isFading = false;
        faderCanvasGroupBlack.blocksRaycasts = false;
    }
    public IEnumerator BlinkOut(float speed)
    {
        isFading = true;
        faderCanvasGroupBlack.blocksRaycasts = true;

        // Fade from black
        float fadeSpeed = Mathf.Abs(faderCanvasGroupBlack.alpha - 0) / (speed);
        while (!Mathf.Approximately(faderCanvasGroupBlack.alpha, 0))
        {
            faderCanvasGroupBlack.alpha = Mathf.MoveTowards(faderCanvasGroupBlack.alpha, 0, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        isFading = false;
        faderCanvasGroupBlack.blocksRaycasts = false;
    }
}