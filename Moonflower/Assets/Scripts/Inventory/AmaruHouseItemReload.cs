using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaruHouseItemReload : MonoBehaviour
{
    private GameObject amaruHouseItem;
    // Start is called before the first frame update
    void Start()
    {
        amaruHouseItem = GameObject.Find("AmaruHouseItem");
        SetActiveTrue(amaruHouseItem);
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
