using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessControl : MonoBehaviour
{
    public PostProcessingProfile passOutProfile;
    public Coroutine passOutCoroutine;

    public void PassOut()
    {
        //Debug.Log("do it");
        //GetComponent<PostProcessingBehaviour>().profile = passOutProfile;

        if (passOutCoroutine == null)
        {
            passOutCoroutine = StartCoroutine(PassOutSequence());
        }
    }
    private IEnumerator PassOutSequence()
    {
        yield return StartCoroutine(SceneController.current.BlinkIn(0.3f));
        GetComponent<PostProcessingBehaviour>().profile = passOutProfile;
        yield return StartCoroutine(SceneController.current.BlinkOut(0.3f));
    }
}
