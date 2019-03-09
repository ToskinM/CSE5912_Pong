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
    //public GameObject playerAnai;
    private GameObject currentPlayer;

    //private CharacterStats anaiStat;
    //private CharacterStats mimbiStat;
    private CharacterStats playerStats;
    private PlayerInventory playerInventory;
    private PlayerSoundEffect soundEffect;
    private FeedbackText feedback;

    public TextMeshProUGUI inventoryAdd;

    public PlayerMovement playerMovement;


    void Start()
    {
        if (!playerAnai)
            playerAnai = gameObject;

        //StartCoroutine(GetAudioManager());
        //inventoryManager = FindObjectOfType<InventoryManager>();
        playerInfo = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        playerStats = GetComponent<CharacterStats>();
        playerInventory = GetComponent<PlayerInventory>();

        anaiStat = LevelManager.current.anai.GetComponent<CharacterStats>();
        mimbiStat = LevelManager.current.mimbi.GetComponent<CharacterStats>();

        playerInventory = playerAnai.GetComponent<PlayerInventory>();
        soundEffect = playerAnai.GetComponent<PlayerSoundEffect>();
        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();

        currentPlayer = PlayerController.instance.GetActivePlayerObject();
        PlayerController.OnCharacterSwitch += SwitchActiveCharacter;
    }

    private GameObject FindClosest()
    {
        allCollidable = GameObject.FindGameObjectsWithTag("Collectable");

            GameObject nearestObj = allCollidable[0];
            float nearest = Vector3.Distance(allCollidable[0].transform.position, currentPlayer.transform.position);
            foreach (GameObject g in allCollidable)
            {
                if (Vector3.Distance(g.transform.position, currentPlayer.transform.position) < nearest)
                {
                    nearest = Vector3.Distance(g.transform.position, currentPlayer.transform.position);
                    nearestObj = g;
                }
                //remove halo if not nearest anymore
                if (g != nearestObj)
                    g.GetComponent<InventoryStat>().SetHalo(false);
            }
            return nearestObj;

    }

    private void DecidePickup()
    {
        GameObject closest = FindClosest();
        if (Vector3.Distance(FindClosest().transform.position, currentPlayer.transform.position) <= distanceToPickup)
        {
            if (closest != null)
            {
                closest.GetComponent<InventoryStat>().SetHalo(true);
            }
            if (Input.GetButtonDown("Pickup"))
            {
                DoPickup(FindClosest());
            }

        }
        else
        {
            if (closest != null)
                closest.GetComponent<InventoryStat>().SetHalo(false);
        }
    }

    public void DoPickup(GameObject obj)
    {
        if (obj.gameObject.tag == "Collectable")
        {
            //Add text update
            inventoryAdd.gameObject.SetActive(true);
            //Remove the text update
            Invoke("DelayMethod", 2f);
            //Get items's health
            int health = obj.GetComponent<InventoryStat>().GetHealth();

            bool objectUsedImmediately = false;
            if (obj.GetComponent<InventoryStat>().AnaiObject)
            {
                string objName = obj.GetComponent<InventoryStat>().Name;
                //TextUpdate(objName + " is collected, " + health + " [health] were add to Anai");
                feedback.ShowText("You have found a " + objName);
               objectUsedImmediately = !anaiStat.AddHealth(health);
            }
            else if (obj.GetComponent<InventoryStat>().MimbiObject)
            {
                string objName = obj.GetComponent<InventoryStat>().Name;
                //TextUpdate(obj.GetComponent<InventoryStat>().Name + " is collected, " + health + " [health] were add to Mimbi");
                feedback.ShowText("You have found a " + objName);
                objectUsedImmediately = !mimbiStat.AddHealth(health);
            }
            else
            {
                //TextUpdate(obj.GetComponent<InventoryStat>().Name + " is collected. ");
                string objName = obj.GetComponent<InventoryStat>().Name;
                feedback.ShowText("You have found a " + objName);
            }
            //Add to inventory
            if(!objectUsedImmediately)
                playerInventory.AddObj(obj.gameObject);
            //Destroy Gameobject after collect
            Destroy(obj.gameObject);
            //Play Pickup audio clip;
            //soundEffect.PlayerPickupSFX();
        }
    }
    void DelayMethod()
    {
        inventoryAdd.gameObject.SetActive(false);
    }

    public void TextUpdate(string s)
    {
        inventoryAdd.SetText(s);
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
