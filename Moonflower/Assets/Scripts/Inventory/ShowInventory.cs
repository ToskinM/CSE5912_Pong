using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowInventory : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;
    public GameObject InventoryPanel;
    public GameObject InvContentPanel; 
    public GameObject Player;

    List<GameObject> items = new List<GameObject>();
    public GameStateController gameController;
    GameObject InvItemTemplate;
    ItemLookup lookup = new ItemLookup(); 
    private PlayerInventory playerInventory;
    private string MoonFlower = "Moon Flower";
    private string WolfApple = "Wolf Apple";
    private bool show;
    private int xOffset = 150;
    private int yOffset = 155;
    private int numCols = 4; 

    // Start is called before the first frame update
    void Awake()
    {
        InvItemTemplate = InvContentPanel.transform.GetChild(0).gameObject; 
        show = false;
        playerInventory = Player.GetComponent<PlayerInventory>();
        gameController = GameObject.Find("Game State Manager").GetComponent<GameStateController>(); 
    }

    public void TextUpdate()
    {
        string displayText = "No of Moonflower: " + playerInventory.GetObjNumber(MoonFlower) + "\nNo of WolfApple: " + playerInventory.GetObjNumber(WolfApple); 
        inventoryText.SetText(displayText);
    }

    public void ItemUpdate()
    {
        DestroyItemIcons(); 
        if (playerInventory.ItemNames.Count > 0)
        {
            bool first = true;
            int currCol = 0;
            int currRow = 0; 
            foreach (string item in playerInventory.ItemNames)
            {
                GameObject newItem; 
                if(first)
                {
                    newItem = InvItemTemplate;
                    currCol = 1; 
                }
                else
                {
                    newItem = Instantiate(InvItemTemplate, InvContentPanel.transform);
                    newItem.transform.position = InvItemTemplate.transform.position + new Vector3(xOffset * currCol, yOffset * currRow, 0);
                    items.Add(newItem); 
                    currCol++; 
                    if(currCol > numCols)
                    {
                        currRow++;
                        currCol = 0; 
                    }

                }

                Image icon = newItem.transform.GetChild(0).GetComponent<Image>();
                TextMeshProUGUI name = newItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                icon.sprite = lookup.GetSprite(item);
                name.text = item; 
            }
        }
        else
        {
            InvItemTemplate.SetActive(false); 
        }
    }

    public void DestroyItemIcons()
    {
        foreach(GameObject item in items)
        {
            Destroy(item); 
        }

        items.Clear(); 
    }

    public void ShowInvList()
    {
        ItemUpdate();
        InventoryPanel.SetActive(true);
        gameController.PauseGame();
        inventoryText.gameObject.SetActive(true);
    }

    public void HideInvList()
    {
        DestroyItemIcons();
        InventoryPanel.SetActive(false);
        gameController.unPauseGame();
        inventoryText.gameObject.SetActive(false);
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
            ShowInvList();
        }
        else
        {
            HideInvList(); 
        }
        TextUpdate();
    }
}
