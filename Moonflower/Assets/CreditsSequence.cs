using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSequence : MonoBehaviour
{
    public float creditsDuration;

    private float time;

    void Update()
    {
        time += Time.deltaTime;

        if (time >= creditsDuration)
        {
            time = 0;

            SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_MAINMENU);
        }
    }
}
