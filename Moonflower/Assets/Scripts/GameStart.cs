using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameStart : MonoBehaviour
{
    IEnumerator Start()
    {
        // Wait for dat awesome logo animation
        yield return new WaitForSeconds(6f);

        // Load main menu
        SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_MAINMENU);
    }
}
