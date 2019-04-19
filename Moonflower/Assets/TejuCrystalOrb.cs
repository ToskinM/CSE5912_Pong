using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TejuCrystalOrb : MonoBehaviour
{
    public GameObject spawnDestroyEffect;

    private Transform tejuTransform;
    private float movementSpeed = 0.01f;

    void Start()
    {
        tejuTransform = GameObject.Find("Teju").transform;
        ObjectPoolController.current.CheckoutTemporary(spawnDestroyEffect, transform, 1);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, tejuTransform.position, movementSpeed);
        movementSpeed *= 1.05f;

        if (Vector3.Distance(transform.position, tejuTransform.position) < 0.5f)
        {
            ObjectPoolController.current.CheckoutTemporary(spawnDestroyEffect, transform, 1);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
