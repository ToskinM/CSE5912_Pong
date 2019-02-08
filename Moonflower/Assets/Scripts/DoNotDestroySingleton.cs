using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroySingleton : MonoBehaviour
{
    private void Awake()
    {
        // If I exist already, destroy me.  There can only be one.
        if (FindObjectOfType<DoNotDestroySingleton>() == this)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
