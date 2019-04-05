using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Unity Tutorial Script
public class SceneController : MonoBehaviour
{
    public static SceneController current;

    public event Action BeforeSceneUnload;
    public event Action AfterSceneLoad;

    [Header("Fade To/From Black")]
    public CanvasGroup faderCanvasGroup;
    public float fadeDuration = 1f;
    private const float pseudoDeltaTime = 0.05f;
    private bool isFading;
    public bool isLoading;

    [Header("Loadscreen")]
    public GameObject loadscreen;
    public CanvasGroup loadscreenCanvasGroup;
    public Slider loadingBar;
    public TextMeshProUGUI progressText;

    private void Start()
    {
        // Only allow one scene controller at a time
        if (current == null)
        {
            DontDestroyOnLoad(gameObject);
            current = this;
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }

        loadscreen.SetActive(false);
        faderCanvasGroup.alpha = 1f;
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

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
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

        // Fade to new scene
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator FadeAndSwitchScenesNoLS(string sceneName)
    {
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
        if (sceneName != Constants.SCENE_MAINMENU) PlayerController.instance.SpawnPlayerObjects();

        // Fade to new scene
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator FadeAndSwitchScenesGameOver(string sceneName)
    {
        isLoading = true;

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
        yield return StartCoroutine(Fade(0f, 5f));
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
        float progress = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            progress = Mathf.Lerp(Mathf.Clamp(progress + 0.1f, 0f, 1f), asyncLoad.progress, Time.deltaTime * 3f);
            loadingBar.value = progress;
            progressText.text = (progress * 100).ToString("F0") + "%";
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
        faderCanvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / (fadeDuration * speedMultiplier);
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * pseudoDeltaTime);
            yield return null;
        }
        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeLoadingBackground(float finalAlpha)
    {
        isFading = true;
        loadscreenCanvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(loadscreenCanvasGroup.alpha - finalAlpha) / (fadeDuration / 3);
        while (!Mathf.Approximately(loadscreenCanvasGroup.alpha, finalAlpha))
        {
            loadscreenCanvasGroup.alpha = Mathf.MoveTowards(loadscreenCanvasGroup.alpha, finalAlpha,
                fadeSpeed * pseudoDeltaTime);
            yield return null;
        }
        isFading = false;
        loadscreenCanvasGroup.blocksRaycasts = false;
    }

    private void HideSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(false);
        if (UISingleton.instance != null) UISingleton.instance.GetComponent<CanvasGroup>().alpha = 0;
        Debug.Log("Hide");
    }

    private void ShowSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(true);
        if (UISingleton.instance != null) UISingleton.instance.GetComponent<CanvasGroup>().alpha = 1;
        Debug.Log("Show");
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
}