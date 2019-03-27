using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaiSingleton : MonoBehaviour
{
    void Awake()
    {
        // If I exist already, destroy me.  There can only be one.
        if (FindObjectOfType<AnaiSingleton>() == this)
        {
            DontDestroyOnLoad(gameObject.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
