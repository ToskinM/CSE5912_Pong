using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCrystal : MonoBehaviour
{
    public bool blocked = true;

    private float rotateSpeed = 0.3f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, rotateSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!blocked && other.gameObject == PlayerController.instance.AnaiObject)
        {
            TriggerMemory();
        }
    }

    private void TriggerMemory()
    {
        Debug.Log("TRIGGERED");
    }
}
