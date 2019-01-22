using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupSequence : MonoBehaviour
{
    public CanvasGroup[] slides;
    public float fadeDuration = 0.8f;

    private SceneController sceneController;

    private IEnumerator Start()
    {
        sceneController = FindObjectOfType<SceneController>();

        for (int i = 0; i < slides.Length; i++)
        {
            yield return StartCoroutine(PlaySlide(slides[i]));
        }

        sceneController.FadeAndLoadScene(Constants.SCENE_MAINMENU);
    }

    private IEnumerator PlaySlide(CanvasGroup slide)
    {
        float fadeSpeed = Mathf.Abs(slide.alpha - 1.0f) / fadeDuration;

        // Fade in
        while (!Mathf.Approximately(slide.alpha, 1.0f))
        {
            slide.alpha = Mathf.MoveTowards(slide.alpha, 1.0f,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        // Fade out
        while (!Mathf.Approximately(slide.alpha, 0.0f))
        {
            slide.alpha = Mathf.MoveTowards(slide.alpha, 0.0f,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
