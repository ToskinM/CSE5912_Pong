using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> InventoryObjs = new List<GameObject>();

    private PlayerHealthDisplay display; 

    void Start()
    {
        display = GameObject.Find("HUD").GetComponent<PlayerHealthDisplay>(); 
    }

    public void AddObj(GameObject obj)
    {
        InventoryObjs.Add(obj);

        if(obj.GetComponent<InventoryStat>().Name.Equals(Constants.MOONFLOWER_PICKUP))
        {
            display.HealPetal();
        }
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
