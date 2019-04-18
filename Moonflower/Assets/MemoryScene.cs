using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MemoryScene : MonoBehaviour
{
    public VideoPlayer VideoPlayer; // Drag & Drop the GameObject holding the VideoPlayer component
    public string SceneName;

    IEnumerator Start()
    {
        // Wait for gag
        yield return new WaitForSeconds(17f);

        // Load main menu
        SceneController.current.FadeAndLoadSceneNoLS("Skyrim");
    }
}
