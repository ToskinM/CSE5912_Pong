using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCrystalShaper : MonoBehaviour
{
    public bool Expanding;
    public float SineOffset;
    private float amplitude;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        amplitude = Expanding ? 0.001f : -0.001f;
        speed = Expanding ? 3: 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.y += Time.timeScale * (amplitude * Mathf.Sin((Time.time + SineOffset) * speed));
        Debug.Log(scale.y);
        transform.localScale = scale;
    }
}
