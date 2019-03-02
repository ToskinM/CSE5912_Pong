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
    public CanvasGroup faderCanvasGroup;
    public float fadeDuration = 1f;

    public GameObject loadingUI;
    public CanvasGroup backgroundCanvasGroup;
    public Slider loadingBar;
    public TextMeshProUGUI progressText;


    private bool isFading;
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

        loadingUI.SetActive(false);
        faderCanvasGroup.alpha = 1f;
        //yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName));
        StartCoroutine(Fade(0f));
    }
    public void FadeAndLoadScene(String sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }
    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        // Fade to black
        yield return StartCoroutine(Fade(1f));
        BeforeSceneUnload?.Invoke();

        // load loading scene
        yield return SceneManager.LoadSceneAsync(Constants.SCENE_LOADING, LoadSceneMode.Additive);

        // Unload previous scene
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // Unload loading scene
        yield return SceneManager.UnloadSceneAsync(Constants.SCENE_LOADING);
        AfterSceneLoad?.Invoke();

        // Fade to new scene
        yield return StartCoroutine(Fade(0f));
    }
    private IEnumerator SwitchScenesNoFadeNoLS(string sceneName)
    {
        BeforeSceneUnload?.Invoke();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActiveNoLS(sceneName));
        AfterSceneLoad?.Invoke();
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        loadingUI.SetActive(true);
        yield return StartCoroutine(FadeLoadingBackground(1f));
        loadingBar.value = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //yield return asyncLoad;

        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            progressText.text = asyncLoad.progress * 100 + "%";
            yield return null;
        }
        StartCoroutine(FadeLoadingBackground(0f));
        loadingUI.SetActive(false);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator LoadSceneAndSetActiveNoLS(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;
        faderCanvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }
    private IEnumerator FadeLoadingBackground(float finalAlpha)
    {
        isFading = true;
        backgroundCanvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(backgroundCanvasGroup.alpha - finalAlpha) / (fadeDuration / 3);
        while (!Mathf.Approximately(backgroundCanvasGroup.alpha, finalAlpha))
        {
            backgroundCanvasGroup.alpha = Mathf.MoveTowards(backgroundCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        isFading = false;
        backgroundCanvasGroup.blocksRaycasts = false;
    }
}