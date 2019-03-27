using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISingleton : MonoBehaviour
{
    void Awake()
    {
        // If I exist already, destroy me.  There can only be one.
        if (FindObjectOfType<UISingleton>() == this)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
