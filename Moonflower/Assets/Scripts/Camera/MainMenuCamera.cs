using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public Transform[] positions;

    public CanvasGroup mainCanvasGroup;
    public CanvasGroup optionsCanvasGroup;

    public CanvasGroup faderCanvasGroup;
    public float fadeDuration = 1f;
    private const float pseudoDeltaTime = 0.025f;
    private bool isFading;
    public bool isMoving;
    private Coroutine fadeCoroutine = null;

    private int currentPosition = 0;

    void Start()
    {
        transform.SetPositionAndRotation(positions[0].position, positions[0].rotation);
        transform.rotation *= Quaternion.Euler(-25, 0, 0);

        // Fade In
        faderCanvasGroup.alpha = 1f;
        faderCanvasGroup.blocksRaycasts = false;
        StartCoroutine(RequestFade(0f, fadeDuration * 3f));
    }

    private void Update()
    {
        if (!Mathf.Approximately(transform.localRotation.eulerAngles.x, positions[currentPosition].localRotation.eulerAngles.x))
        {
            Debug.Log(transform.localRotation + "derp" + positions[currentPosition].localRotation);
            //Quaternion.Lerp(transform.rotation, positions[currentPosition].rotation, Time.deltaTime * 10);
            Quaternion.RotateTowards(transform.localRotation, positions[currentPosition].localRotation, 1);
        }

        //if (!isMoving)
        //{
        //    isMoving = true;
        //    Debug.Log("rotate   ");

        //    StartCoroutine(Sway());
        //}
    }

    private IEnumerator Sway()
    {
        Vector3 randomSwayAngle = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0f);
        Quaternion newAngle = transform.rotation * Quaternion.Euler(randomSwayAngle);

        while (transform.rotation != newAngle)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, newAngle, Time.deltaTime * 1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newAngle, Time.deltaTime * 1f);
            yield return null;
        }
        while (transform.rotation != positions[currentPosition].rotation)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, positions[currentPosition].rotation, Time.deltaTime * 1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, positions[currentPosition].rotation, Time.deltaTime * 1f);
            yield return null;
        }
        isMoving = false;
    }

    private IEnumerator RequestFade(float finalAlpha, float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
            yield return fadeCoroutine = StartCoroutine(Fade(finalAlpha, fadeDuration));
        }
        else
        {
            yield return fadeCoroutine = StartCoroutine(Fade(finalAlpha, fadeDuration));
        }
    }

    public void SetPosition(int position)
    {
        StartCoroutine(Transition(position));
    }

    private IEnumerator Transition(int position)
    {
        yield return StartCoroutine(RequestFade(1f, fadeDuration));

        transform.SetPositionAndRotation(positions[position].position, positions[position].rotation);
        currentPosition = position;

        yield return StartCoroutine(RequestFade(0f, fadeDuration));

    }

    // Fade to/from black
    private IEnumerator Fade(float finalAlpha, float duration)
    {
        isFading = true;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / duration;
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * pseudoDeltaTime);
            mainCanvasGroup.alpha = Mathf.MoveTowards(mainCanvasGroup.alpha, 1 - finalAlpha, fadeSpeed * pseudoDeltaTime);
            optionsCanvasGroup.alpha = Mathf.MoveTowards(optionsCanvasGroup.alpha, 1 - finalAlpha, fadeSpeed * pseudoDeltaTime);
            yield return null;
        }

        isFading = false;
    }
}
