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
        transform.position += new Vector3(0, 0.15f, 0);

        // Fade In
        faderCanvasGroup.alpha = 1f;
        mainCanvasGroup.alpha = 0f;
        faderCanvasGroup.blocksRaycasts = false;
        StartCoroutine(RequestFade(0f, fadeDuration * 3f));
    }

    private void Update()
    {
        // Initial decend effect
        if (!(Mathf.Abs(transform.position.y - positions[currentPosition].position.y) < 0.01f))
        {
            transform.position = Vector3.MoveTowards(transform.position, positions[currentPosition].position, Mathf.Abs(positions[currentPosition].position.y - transform.position.y) * 0.01f);
        }

        //if (!isMoving)
        //{
        //    isMoving = true;
        //    StartCoroutine(Sway());
        //}
    }

    private IEnumerator Sway()
    {
        Vector3 randomSway = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0f);
        Vector3 newPosition = transform.position + randomSway;
        while (transform.position != newPosition)
        {
            Debug.Log("derp");
            //transform.rotation = Quaternion.Lerp(transform.rotation, newAngle, Time.deltaTime * 1f);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, Vector3.Magnitude(newPosition - transform.position));
            yield return null;
        }
        while (transform.position != positions[currentPosition].position)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, positions[currentPosition].rotation, Time.deltaTime * 1f);
            transform.position = Vector3.MoveTowards(transform.position, positions[currentPosition].position, Vector3.Magnitude(positions[currentPosition].position - transform.position));
            yield return null;
        }

        //Vector3 randomSwayAngle = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0f);
        //Quaternion newAngle = transform.rotation * Quaternion.Euler(randomSwayAngle);

        //while (transform.rotation != newAngle)
        //{
        //    Debug.Log("derp");
        //    //transform.rotation = Quaternion.Lerp(transform.rotation, newAngle, Time.deltaTime * 1f);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, newAngle, Vector3.Angle(transform.rotation.eulerAngles, newAngle.eulerAngles) * 1f + 0.001f);
        //    yield return null;
        //}
        //while (transform.rotation != positions[currentPosition].rotation)
        //{
        //    //transform.rotation = Quaternion.Lerp(transform.rotation, positions[currentPosition].rotation, Time.deltaTime * 1f);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, positions[currentPosition].rotation, Vector3.Angle(transform.rotation.eulerAngles, newAngle.eulerAngles) * 1f + 0.001f);
        //    yield return null;
        //}
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
