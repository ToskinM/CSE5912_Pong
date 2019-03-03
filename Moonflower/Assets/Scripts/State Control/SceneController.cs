﻿using System;
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
    private bool isFading;

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
        StartCoroutine(Fade(0f));
    }

    // Call these to switch scenes (NOT ADDITIVE)
    //
    public void FadeAndLoadScene(String sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }
    // (without sexy loadscreen)
    public void FadeAndLoadSceneNoLS(String sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenesNoLS(sceneName));
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

    private IEnumerator FadeAndSwitchScenesNoLS(string sceneName)
    {
        // Fade to black
        yield return StartCoroutine(Fade(1f));
        BeforeSceneUnload?.Invoke();

        // load loading scene
        yield return SceneManager.LoadSceneAsync(Constants.SCENE_LOADING, LoadSceneMode.Additive);

        // Unload previous scene (without loadscreen)
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActiveNoLS(sceneName));

        // Unload loading scene
        yield return SceneManager.UnloadSceneAsync(Constants.SCENE_LOADING);
        AfterSceneLoad?.Invoke();

        // Fade to new scene
        yield return StartCoroutine(Fade(0f));
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
            progressText.text = asyncLoad.progress * 100 + "%";
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
        loadscreenCanvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(loadscreenCanvasGroup.alpha - finalAlpha) / (fadeDuration / 3);
        while (!Mathf.Approximately(loadscreenCanvasGroup.alpha, finalAlpha))
        {
            loadscreenCanvasGroup.alpha = Mathf.MoveTowards(loadscreenCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        isFading = false;
        loadscreenCanvasGroup.blocksRaycasts = false;
    }
}