using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInventory : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;
    public GameObject player;
    private PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    public void textUpdate()
    {
        string displayText = "No of Moonflower: " + playerInventory.GetNoMoonFlower() +"\n No of WolfApple" + playerInventory.GetNoWolfApple(); 
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
            inventoryText.gameObject.SetActive(false);
        inventoryText.gameObject.transform.position = Camera.main.WorldToScreenPoint(new Vector3(player.transform.position.x, player.transform.position.y+3, player.transform.position.z));
        textUpdate();
    }
}
