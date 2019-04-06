using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCrystal : MonoBehaviour
{
    private float rotateSpeed = 0.3f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, rotateSpeed, 0);
    }
}
