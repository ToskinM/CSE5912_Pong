using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageItemReload : MonoBehaviour
{
    private GameObject villageItem;
    private GameObject amaruHouseItem;
    private GameObject naiaHouseItem;
    // Start is called before the first frame update
    void Start()
    {
        villageItem = GameObject.Find("VillageItem");
        amaruHouseItem = GameObject.Find("AmaruHouseItems");
        naiaHouseItem = GameObject.Find("NaiaHouseItems");
        SetActiveTrue(villageItem);
        //ExecuteAfterTime(1f);
        //Debug.Log("start");

        //if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE)
        //    SetActiveTrue(villageItem);
        //else if (SceneManager.GetActiveScene().name == Constants.SCENE_AMARUHOUSE)
        //    SetActiveTrue(amaruHouseItem);
        //else if (SceneManager.GetActiveScene().name == Constants.SCENE_NAIAHOUSE)
        //    SetActiveTrue(amaruHouseItem);
        //else
            //Debug.Log(SceneManager.GetActiveScene().name + "" + Constants.SCENE_VILLAGE);


    }


    public void SetActiveTrue(GameObject SceneItem)
    {
        if (SceneItem != null)
        {
            foreach (Transform child in SceneItem.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
