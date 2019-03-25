using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private List<GameObject> currentCollisions = new List<GameObject>();
    private GameObject[] allCollidable;
    public int distanceToPickup = 5;

    public CharacterStats Stats { get; private set; }
    private GameObject currentPlayer;

    private CharacterStats playerStats;
    private PlayerInventory playerInventory;
    private PlayerSoundEffect soundEffect;
    private InteractionPopup interaction;
    private FeedbackText feedback;

    //public TextMeshProUGUI inventoryAdd;

    void Start()
    {
        //StartCoroutine(GetAudioManager());
        //inventoryManager = FindObjectOfType<InventoryManager>();
        playerStats = GetComponent<CharacterStats>();
        playerInventory = GetComponent<PlayerInventory>();

        soundEffect = PlayerController.instance.GetActivePlayerObject().GetComponent<PlayerSoundEffect>();
        interaction = GameObject.Find("HUD").transform.GetChild(7).GetComponent<InteractionPopup>();

        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        Debug.Log(PlayerController.instance.GetActivePlayerObject());
        currentPlayer = PlayerController.instance.GetActivePlayerObject();
        Debug.Log("currentPlayer");
        PlayerController.OnCharacterSwitch += SwitchActiveCharacter; 
    }

    private GameObject FindClosest()
    {
        allCollidable = GameObject.FindGameObjectsWithTag("Collectable");

        if (allCollidable.Length > 0)
        {
            GameObject nearestObj = allCollidable[0];
            float nearest = Vector3.Distance(allCollidable[0].transform.position, currentPlayer.transform.position);
            foreach (GameObject g in allCollidable)
            {
                if (Vector3.Distance(g.transform.position, currentPlayer.transform.position) < nearest)
                {
                    nearest = Vector3.Distance(g.transform.position, currentPlayer.transform.position);
                    nearestObj = g;
                }
                g.GetComponent<InventoryStat>().SetHalo(false);

            }
            return nearestObj;
        }
        return null;
    }

    private void DecidePickup()
    {
        GameObject closest = FindClosest();
        if (closest != null)
        {
            float dist = Vector3.Distance(closest.transform.position, currentPlayer.transform.position);
            if (dist <= distanceToPickup && !interaction.NotAllowed)
            {
                if (closest != null)
                {
                    InventoryStat stat = closest.GetComponent<InventoryStat>();
                    if (!(stat.AnaiObject && !PlayerController.instance.AnaiIsActive()) && !(stat.MimbiObject && PlayerController.instance.AnaiIsActive()))
                    {
                        closest.GetComponent<InventoryStat>().SetHalo(true);
                        interaction.EnableItem(dist);
                    }
                }
                if (Input.GetButtonDown("Interact"))
                {
                    DoPickup(closest);
                    interaction.DisableItem();
                }

            }
            else
            {
                if (closest != null)
                    closest.GetComponent<InventoryStat>().SetHalo(false);
                interaction.DisableItem();
            }
        }
        
    }

    private void CollectLifeObject(GameObject obj, InventoryStat stat)
    {
        //Get items's health
        int health = obj.GetComponent<InventoryStat>().GetHealth();

        bool objectUsedImmediately = false;
        bool anaiObjectMatch = stat.AnaiObject && PlayerController.instance.AnaiIsActive(); //(currentPlayer.Equals(PlayerController.instance.AnaiObject));
        bool mimbiObjectMatch = stat.MimbiObject && !PlayerController.instance.AnaiIsActive();

        if (anaiObjectMatch || mimbiObjectMatch)
        {
            //Add text update
            //inventoryAdd.gameObject.SetActive(true);
            //Remove the text update
            Invoke("DelayMethod", 2f);
            string objName = obj.GetComponent<InventoryStat>().Name;
            feedback.ShowText("You have found a " + objName.ToLower());
            objectUsedImmediately = playerStats.AddHealth(health);

            //Add to inventory
            if (!objectUsedImmediately)
                playerInventory.AddObj(obj.gameObject);

            //Destroy Gameobject after collect
            Destroy(obj.gameObject);
            //Play Pickup audio clip;
            soundEffect.PlayerPickupSFX();
        } 
    }

    public void DoPickup(GameObject obj)
    {
        if (obj.gameObject.tag == "Collectable")
        {
            InventoryStat stat = obj.GetComponent<InventoryStat>();
            if (stat.AnaiObject || stat.MimbiObject)
            {
                CollectLifeObject(obj,stat);
            }
            else
            {
                //Add text update
                //inventoryAdd.gameObject.SetActive(true);
                //Remove the text update
                Invoke("DelayMethod", 2f);
                string objName = obj.GetComponent<InventoryStat>().Name;
                feedback.ShowText("You have found a " + objName.ToLower());

                //Add to inventory
                playerInventory.AddObj(obj.gameObject);
                //Destroy Gameobject after collect
                Destroy(obj.gameObject);
                //Play Pickup audio clip;
                soundEffect.PlayerPickupSFX();
            }

        }
    }
    void DelayMethod()
    {
        //inventoryAdd.gameObject.SetActive(false);
    }

    public void TextUpdate(string s)
    {
        //inventoryAdd.SetText(s);
    }

    // Update is called once per frame
    void Update()
    {
        DecidePickup();
    }

    void SwitchActiveCharacter(PlayerController.PlayerCharacter activeChar)
    {
        currentPlayer = PlayerController.instance.GetActivePlayerObject();
    }
}
