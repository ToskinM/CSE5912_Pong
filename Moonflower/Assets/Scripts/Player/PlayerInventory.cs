using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> MoonFlowers = new List<GameObject>();
    public List<GameObject> WolfApples = new List<GameObject>();
    //private InventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        //inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void AddMoonFlower(GameObject i)
    {
        MoonFlowers.Add(i);
    }
    public void AddWolfApple(GameObject i)
    {
        WolfApples.Add(i);
    }
    public int GetNoMoonFlower()
    {
        return MoonFlowers.Count;
    }
    public int GetNoWolfApple()
    {
        return WolfApples.Count;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
