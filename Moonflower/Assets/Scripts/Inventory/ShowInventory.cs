using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInventory : MonoBehaviour
{
    //public TextMeshProUGUI inventoryText;
    public GameObject inventoryPanel;
    public GameObject player;
    private PlayerInventory playerInventory;
    private string MoonFlower = "Moon Flower";
    private string WolfApple = "Wolf Apple";
    private bool show;
    // Start is called before the first frame update
    void Start()
    {
        show = false;
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    public void textUpdate()
    {
        string displayText = "No of Moonflower: " + playerInventory.getObjNumber(MoonFlower) + "\nNo of WolfApple: " + playerInventory.getObjNumber(WolfApple); 
        //inventoryText.SetText(displayText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            show = !show;
            //inventoryText.gameObject.SetActive(true);

        }
        if (show)
        {
            //inventoryText.gameObject.SetActive(false);
            inventoryPanel.SetActive(true);
        }
        else
            inventoryPanel.SetActive(false);
        textUpdate();
    }
}
