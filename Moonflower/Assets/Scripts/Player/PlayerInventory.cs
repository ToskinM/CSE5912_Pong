using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> InventoryObjs = new List<GameObject>();
    public List<string> ItemNames = new List<string>();
    public Dictionary<string, GameObject> ItemObjMap = new Dictionary<string, GameObject>();
    public Dictionary<string, int> ItemAmountMap = new Dictionary<string, int>(); 

    //public GameObject pickupArea;

    private PlayerHealthDisplay display; 

    void Start()
    {
        display = GameObject.Find("HUD").GetComponent<PlayerHealthDisplay>();
        //pickupArea.SetActive(false);
    }

    public void AddObj(GameObject obj)
    {
        string objName = obj.GetComponent<InventoryStat>().Name;
        if (ItemNames.Contains(objName))
        {
            int num = ItemAmountMap[objName] + 1;
            ItemAmountMap.Remove(objName);
            ItemAmountMap.Add(objName, num);
        }
        else
        { 
            InventoryObjs.Add(obj);
            ItemNames.Add(objName);
            ItemObjMap.Add(objName, obj);
            ItemAmountMap.Add(objName, 1);
        }

        if(objName.Equals(ItemLookup.MOONFLOWER_NAME))
        {
            display.Heal();
        }

        if (objName.Equals(ItemLookup.WOLFAPPLE_NAME))
        {
            display.Heal(); 
        }
    }

    public int GetObjNumber(string obj)
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
        //DetectPickup();
    }

    //public void DetectPickup()
    //{
    //    if (Input.GetButtonDown("Pickup"))
    //    {
    //        pickupArea.SetActive(true);
    //    }
    //    else if (Input.GetButtonUp("Pickup"))
    //    {
    //        pickupArea.SetActive(false);
    //    }
    //}
}
