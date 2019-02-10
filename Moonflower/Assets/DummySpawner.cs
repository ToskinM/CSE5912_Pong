using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    public GameObject dummy;
    public float delay = 0f;

    private float timer;

    // Update is called once per frame
    void Start()
    {
        timer = delay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            GameObject spawned = ObjectPoolController.current.Checkout(dummy, transform);
            if (spawned)
                spawned.transform.parent = gameObject.transform;
            timer = delay;
        }
    }
}
