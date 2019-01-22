using UnityEngine;
using UnityEngine.SceneManagement;

public class EnsurePersistentSceneLoaded : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetSceneByName(Constants.SCENE_PERSISTENT) != null)
        {
            SceneManager.LoadSceneAsync(Constants.SCENE_PERSISTENT, LoadSceneMode.Additive);
        }
    }
}
