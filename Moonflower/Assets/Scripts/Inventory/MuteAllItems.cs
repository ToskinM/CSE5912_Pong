using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAllItems : MonoBehaviour
{
    private GameObject villageItem;
    private GameObject anaiHouseItem;
    private GameObject amaruHouseItem;
    private GameObject naiaHouseItem;

    // Start is called before the first frame update
    void Start()
    {
        villageItem = GameObject.Find("VillageItem");
        amaruHouseItem = GameObject.Find("AmaruHouseItems");
        naiaHouseItem = GameObject.Find("NaiaHouseItems");

        //villageItem.SetActive(false);
        //amaruHouseItem.SetActive(false);
        //naiaHouseItem.SetActive(false);


        SetActiveFalse(villageItem);
        SetActiveFalse(amaruHouseItem);
        SetActiveFalse(naiaHouseItem);

    }

    public void SetActiveFalse(GameObject SceneItem)
    {
        if (SceneItem != null)
        {
            foreach (Transform child in SceneItem.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
