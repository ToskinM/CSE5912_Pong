using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSequence : MonoBehaviour
{
    public float startDelay = 1f;
    public float scrollSpeed = 0.5f;

    public CanvasGroup[] displayGroups;
    public Transform startPoint;
    public Transform midPoint;
    public Transform endPoint;

    private CanvasGroup currentlyDisplayedGroup;

    void Start()
    {
        // Hide all groups to begin with
        for (int i = 0; i < displayGroups.Length; i++)
        {
            displayGroups[i].alpha = 0;
        }

        StartCoroutine(BeginSequence());
    }

    void Update()
    {

    }

    private IEnumerator BeginSequence()
    {
        // Initial delay
        yield return new WaitForSeconds(startDelay);

        // 
        for (int i = 0; i < displayGroups.Length; i++)
        {
            yield return StartCoroutine(GroupPass(displayGroups[i]));
        }
    }

    private IEnumerator GroupPass(CanvasGroup group)
    {
        Transform groupTransform = group.transform;

        while (Vector3.Distance(group.transform.position, endPoint.position) > 0.1f)
        {
            groupTransform.position = Vector3.MoveTowards(groupTransform.position, endPoint.position, scrollSpeed);
            group.alpha = Mathf.Clamp((1 - Vector3.Distance(groupTransform.position, midPoint.position) / Vector3.Distance(startPoint.position, midPoint.position)), 0f, 1f);

            yield return null;
        }
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float finalAlpha, float fadeDuraton = 1)
    {
        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuraton;
        while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
