using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialRandomizer : MonoBehaviour
{
    public Material[] materials;
    public Renderer renderer;

    void Start()
    {
        renderer.material = materials[(int)Random.Range(0f, materials.Length - 1)];
    }
}
