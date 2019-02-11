using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> InventoryObjs = new List<GameObject>();

    void Start()
    {
    }

    public void AddObj(GameObject obj)
    {
        InventoryObjs.Add(obj);
    }
    public int getObjNumber(string obj)
    {
        int count = 0;
        foreach (GameObject inventoryObj in InventoryObjs)
        {
            if (inventoryObj.GetComponent<InventoryStat>().Name == obj)
            {
                count++;
            }
        }
        return count;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
