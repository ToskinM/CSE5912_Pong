using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaiItemReload : MonoBehaviour
{
    private GameObject anaiHouseItem;
    // Start is called before the first frame update
    void Start()
    {
        anaiHouseItem = GameObject.Find("AnaiHouseItems");
        SetActiveTrue(anaiHouseItem);
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
}
