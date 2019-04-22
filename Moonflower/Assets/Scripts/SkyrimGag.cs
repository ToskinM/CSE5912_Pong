using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyrimGag : MonoBehaviour
{
    IEnumerator Start()
    {
        // Wait for gag
        yield return new WaitForSeconds(17f);

        // Load main menu
        SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_CREDITS);
    }
}
