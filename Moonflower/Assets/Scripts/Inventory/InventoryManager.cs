using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory[] items;
    public static InventoryManager instance;
   
    // Start is called before the first frame update
    void Start()
    {

    }

    public float getHealth(string name)
    {
        Inventory i = Array.Find(items, Inventory => Inventory.name == name);
        if (i == null)
        {
            Debug.Log("there is nth");
        }
        return i.Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
