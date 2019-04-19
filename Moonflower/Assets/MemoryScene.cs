using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MemoryScene : MonoBehaviour
{

    IEnumerator Start()
    {
        // Wait for gag
        yield return new WaitForSeconds(20f);

        // Load main menu
        SceneController.current.FadeAndLoadSceneNoLS("Skyrim");
    }
}
