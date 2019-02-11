using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInventory : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;
    public GameObject player;
    private PlayerInventory playerInventory;
    private string MoonFlower = "Moon Flower";
    private string WolfApple = "Wolf Apple";
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    public void textUpdate()
    {
        string displayText = "No of Moonflower: " + playerInventory.getObjNumber(MoonFlower) + "\nNo of WolfApple: " + playerInventory.getObjNumber(WolfApple); 
        inventoryText.SetText(displayText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryText.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            inventoryText.gameObject.SetActive(false);
        }
        textUpdate();
    }
}
