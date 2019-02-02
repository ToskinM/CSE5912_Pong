using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public string name;
    public GameObject gameObject;

    [Range(0, 100)]
    public int Strength;
    [Range(0, 100)]
    public int Attack;
    [Range(0, 100)]
    public int Defense;
    [Range(0, 100)]
    public int Health;


}
