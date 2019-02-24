﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowInventory : MonoBehaviour
{
    //public TextMeshProUGUI inventoryText;
    public GameObject InventoryPanel;
    public GameObject InvContentPanel; 
    public GameObject Player;
    public Button InvoButton; 

    List<GameObject> items = new List<GameObject>();
    List<string> names = new List<string>(); 
    public GameStateController gameController;
    GameObject InvItemTemplate;
    ItemLookup lookup = new ItemLookup(); 
    private PlayerInventory playerInventory;
    private bool show;
    private float xOffset;
    private float yOffset;
    private int numCols = 4;
    private bool inEnglish = false; 
    private bool toggleEnabled = false;
    private int toggleMax = 30;
    private int toggleCount = 0; 


    // Start is called before the first frame update
    void Awake()
    {
        xOffset = Screen.width / 5;
        yOffset = Screen.height / 2.5f;
        InvItemTemplate = InvContentPanel.transform.GetChild(0).gameObject; 
        show = false;
        playerInventory = Player.GetComponent<PlayerInventory>();
        gameController = GameObject.Find("Game State Manager").GetComponent<GameStateController>();
        InvoButton.onClick.AddListener(showInv);
   }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && toggleEnabled)
        {
            toggleEnabled = false; 
            inEnglish = !inEnglish; 
            foreach(GameObject item in items)
            {
                string itemName = names[items.IndexOf(item)];
                if (inEnglish)
                {
                    item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemName;
                }
                else
                {
                    item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = lookup.GetGuaraniName(itemName);
                }

            }
        }
        else
        {
            toggleCount++; 
            if(toggleCount > toggleMax)
            {
                toggleCount = 0;
                toggleEnabled = true; 
            }
        }
    }

    private void showInv()
    {
        show = !show; 
        if(show)
        {
            ShowInvList();
        }
        else
        {
            HideInvList(); 
        }
    }

    //public void TextUpdate()
    //{
    //    string displayText = "No of Moonflower: " + playerInventory.GetObjNumber(MoonFlower) + "\nNo of WolfApple: " + playerInventory.GetObjNumber(WolfApple); 
    //    //inventoryText.SetText(displayText);
    //}

    public void ItemUpdate()
    {
        InvItemTemplate.SetActive(true);
        DestroyItemIcons(); 

        if (playerInventory.ItemNames.Count > 0)
        {
            //bool first = true;
            int currCol = 0;
            int currRow = 0;
            float heightDim = 10; 
            foreach (string item in playerInventory.ItemNames)
            {
                GameObject newItem;
                //if(first)
                //{
                //    newItem = InvItemTemplate;
                //    currCol = 1;
                //    first = false; 
                //}
                //else
                {
                    newItem = Instantiate(InvItemTemplate, InvContentPanel.transform);
                    newItem.transform.position = InvItemTemplate.transform.position + new Vector3(xOffset * currCol, -yOffset * currRow, 0);
                    items.Add(newItem); 
                    currCol++; 
                    if(currCol > numCols)
                    {
                        currRow++;
                        currCol = 0; 
                    }

                }
                names.Add(item);

                Image icon = newItem.transform.GetChild(0).GetComponent<Image>();
                TextMeshProUGUI itemName = newItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI itemNum = newItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

                heightDim = icon.gameObject.GetComponent<RectTransform>().rect.height * 2.3f;

                icon.sprite = lookup.GetSprite(item);
                if (inEnglish)
                    itemName.text = item;
                else
                    itemName.text = lookup.GetGuaraniName(item); 

                int numItem = playerInventory.ItemAmountMap[item];
                if(numItem > 1)
                    itemNum.text = "" + numItem; 
            }

            RectTransform rect = InvContentPanel.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, currRow * heightDim); 
        }
        else
        {

            //RectTransform rect = InvContentPanel.GetComponent<RectTransform>();
            //rect.sizeDelta = new Vector2(rect.rect.width, yOffset);

        }
        InvItemTemplate.SetActive(false);

    }

    public void DestroyItemIcons()
    {
        while(items.Count > 0)
        {
            Destroy(items[0]);
            items.RemoveAt(0); 
        }

        items.Clear();
        names.Clear();  
    }

    public void ShowInvList()
    {
        ItemUpdate();
        InventoryPanel.SetActive(true);
        gameController.PauseGame();
        //inventoryText.gameObject.SetActive(true);
    }

    public void HideInvList()
    {
        DestroyItemIcons();
        InventoryPanel.SetActive(false);
        gameController.unPauseGame();
        //inventoryText.gameObject.SetActive(false);
    }


}
