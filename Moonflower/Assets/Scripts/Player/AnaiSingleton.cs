using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaiSingleton : MonoBehaviour
{
    public static AnaiSingleton instance;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
