using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Button consumable;
    public Button key;
    public Button material;

    public GameObject consumableObj;
    public GameObject keyObj;
    public GameObject materialObj;

    // Start is called before the first frame update
    void Start()
    {
        consumable.onClick.AddListener(ShowConsumablePannel);
        key.onClick.AddListener(ShowKeyPannel);
        material.onClick.AddListener(ShowMaterialPannel);
    }

    public void ShowConsumablePannel()
    {
        consumableObj.SetActive(true);
        keyObj.SetActive(false);
        materialObj.SetActive(false);
    }
    public void ShowKeyPannel()
    {
        consumableObj.SetActive(false);
        keyObj.SetActive(true);
        materialObj.SetActive(false);
    }
    public void ShowMaterialPannel()
    {
        consumableObj.SetActive(false);
        keyObj.SetActive(false);
        materialObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
