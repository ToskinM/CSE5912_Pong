using UnityEngine;
using UnityEngine.SceneManagement;

public class EnsurePersistentSceneLoaded : MonoBehaviour
{
    void Start()
    {
        Scene scene = SceneManager.GetSceneByName(Constants.SCENE_PERSISTENT);
        if (scene.name != Constants.SCENE_PERSISTENT)
        {
            SceneManager.LoadSceneAsync(Constants.SCENE_PERSISTENT, LoadSceneMode.Additive);
        }
    }
}
