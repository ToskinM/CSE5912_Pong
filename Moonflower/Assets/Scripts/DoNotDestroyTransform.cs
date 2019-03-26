using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroyTransform : MonoBehaviour
{
    void Awake()
    {
        // If I exist already, destroy me.  There can only be one.
        if (FindObjectOfType<DoNotDestroyTransform>() == this)
        {
            DontDestroyOnLoad(gameObject.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
