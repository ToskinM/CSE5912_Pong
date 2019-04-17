using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaiaHouseItemReload : MonoBehaviour
{
    private GameObject naiaHouseItem;
    // Start is called before the first frame update
    void Start()
    {
        naiaHouseItem = GameObject.Find("NaiaHouseItems");
        SetActiveTrue(naiaHouseItem);
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
