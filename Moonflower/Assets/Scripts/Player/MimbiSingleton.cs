using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimbiSingleton : MonoBehaviour
{
    public static MimbiSingleton instance;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            // MIMBI IS A HIGH MAINTENANCE DOGE
            if (UnityEngine.SceneManagement.SceneManager.sceneCount > 1)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(1).name == Constants.SCENE_ANAIHOUSE)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
