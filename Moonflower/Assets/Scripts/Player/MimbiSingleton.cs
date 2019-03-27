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
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
}
