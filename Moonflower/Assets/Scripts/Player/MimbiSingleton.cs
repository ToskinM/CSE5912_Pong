using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimbiSingleton : MonoBehaviour
{
    void Awake()
    {
        // If I exist already, destroy me.  There can only be one.
        if (FindObjectOfType<MimbiSingleton>() == this)
        {
            DontDestroyOnLoad(gameObject.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
