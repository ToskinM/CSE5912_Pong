using System.Collections;
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
    public InteractionPopup interaction;

    //public Button InvoButton; 

    List<GameObject> items = new List<GameObject>();
    List<string> names = new List<string>(); 
    public GameStateController gameController;
    GameObject InvItemTemplate;
    ItemLookup lookup = new ItemLookup(); 
    private PlayerInventory playerInventory;
    public bool Shown = false;
    private bool isGift = false; 
    private bool buttonActive = false;
    INPCController receiverController;


    private float xOffset;
    private float yOffset;
    private int numCols = 4;
    private bool inEnglish = false; 
    private bool toggleEnabled = false;
    private int toggleMax = 30;
    private int toggleCount = 0; 


    void Awake()
    {
        xOffset = Screen.width / 5;
        yOffset = Screen.height / 2.5f;
        InvItemTemplate = InvContentPanel.transform.GetChild(0).gameObject; 
        Shown = false;
        playerInventory = Player.GetComponent<PlayerInventory>();
        gameController = GameStateController.current;
        //InvoButton.onClick.AddListener(showInv);
   }

    void Update()
    {
        if(!isGift && Input.GetKeyDown(KeyCode.I))
        {
            ToggleInv(); 
            toggleEnabled = false;

        }
        if(isGift && Input.GetKeyDown(KeyCode.X))
        {
            HideInvList(); 
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inEnglish = !inEnglish;
            foreach (GameObject item in items)
            {
                string itemName = names[items.IndexOf(item)];
                if (inEnglish)
                {
                    item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemName.ToLower();
                }
                else
                {
                    item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = lookup.GetGuaraniName(itemName);
                }

            }
        }
    }

    public bool HasInv()
    {
        return playerInventory.ItemNames.Count > 0; 
    }

    public void ShowGiftInventory(INPCController controller)
    {
        Shown = true;
        isGift = true; 
        buttonActive = true;
        ShowInvList();
        receiverController = controller;
        GameStateController.current.SetMouseLock(false);
    }

    private void giftToNPC(string objName)
    {
        HideInvList();
        buttonActive = false;
        receiverController.Gift(objName);
        receiverController = null;
        playerInventory.RemoveObj(objName); 
    }

    private void ToggleInv()
    {
        if(!Shown)
        {
            GameStateController.current.SetMouseLock(false);
            ShowInvList();
            Shown = true;
            foreach (GameObject item in items)
            {
                string itemName = names[items.IndexOf(item)];
                item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = lookup.GetGuaraniName(itemName);

            }
        }
        else
        {
            GameStateController.current.SetMouseLock(true);
            HideInvList();
            Shown = false;
        }
    }

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
                if (!isGift || !lookup.IsLifeObject(item))
                {
                    //make a copy of the inventory item template 
                    GameObject newItem;
                    newItem = Instantiate(InvItemTemplate, InvContentPanel.transform);
                    newItem.transform.position = InvItemTemplate.transform.position + new Vector3(xOffset * currCol, -yOffset * currRow, 0);
                    items.Add(newItem);
                    //update place in inventory grid
                    currCol++;
                    if (currCol > numCols)
                    {
                        currRow++;
                        currCol = 0;
                    }


                    names.Add(item);

                    //get all components of the template
                    Image icon = newItem.transform.GetChild(0).GetComponent<Image>();
                    TextMeshProUGUI itemName = newItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI itemNum = newItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

                    Button invButton = newItem.transform.GetChild(3).GetComponent<Button>();

                    //set up or deactivate button accordingly
                    if (buttonActive)
                        invButton.onClick.AddListener(delegate { giftToNPC(item); });
                    else
                        invButton.gameObject.SetActive(false);

                    //get for content scroll info
                    heightDim = icon.gameObject.GetComponent<RectTransform>().rect.height * 2.3f;

                    icon.sprite = lookup.GetSprite(item);
                    if (inEnglish)
                        itemName.text = item.ToLower();
                    else
                        itemName.text = lookup.GetGuaraniName(item);

                    int numItem = playerInventory.ItemAmountMap[item];
                    if (numItem > 1)
                        itemNum.text = "" + numItem;
                }
            }

            RectTransform rect = InvContentPanel.GetComponent<RectTransform>();
            if ((currRow == 2 && currCol > 1) || currRow > 2)
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 1.2f*currRow * heightDim); 
            else
                rect.sizeDelta = new Vector2(0, 0);
        }
        else
        {
            RectTransform rect = InvContentPanel.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 0);
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
        interaction.NotAllowed = true; 
        ItemUpdate();
        InventoryPanel.SetActive(true);
        GameStateController.current.PauseGame();
        Shown = true; 
        //inventoryText.gameObject.SetActive(true);
    }

    public void HideInvList()
    {
        interaction.NotAllowed = false; 
        DestroyItemIcons();
        InventoryPanel.SetActive(false);
        GameStateController.current.UnpauseGame();
        //inventoryText.gameObject.SetActive(false);
        isGift = false;
        Shown = false;
        buttonActive = false;  
    }


}
