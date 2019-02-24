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
    public GameObject PlayerAnai;
    public GameObject PlayerMimbi;
    private GameObject CurrentPlayer;

    private CharacterStats playerStat;
    //private InventoryManager inventoryManager;
    private PlayerInventory playerInventory;
    private AudioManager audioManager;

    public TextMeshProUGUI inventoryAdd;

    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetAudioManager());
        //inventoryManager = FindObjectOfType<InventoryManager>();
        playerStat = PlayerAnai.GetComponent<CharacterStats>();
        playerInventory = PlayerAnai.GetComponent<PlayerInventory>();
        CurrentPlayer = PlayerAnai;
    }

    private IEnumerator GetAudioManager()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            yield return null;
        }
    }

    private GameObject FindClosest()
    {
        allCollidable = GameObject.FindGameObjectsWithTag("Collectable");
        GameObject nearestObj = allCollidable[0];
        float nearest = Vector3.Distance(allCollidable[0].transform.position, CurrentPlayer.transform.position);
        foreach (GameObject g in allCollidable)
        {
            if (Vector3.Distance(g.transform.position, CurrentPlayer.transform.position) < nearest)
            {
                nearest = Vector3.Distance(g.transform.position, CurrentPlayer.transform.position);
                nearestObj = g;
            }
            //remove halo if not nearest anymore
            if (g!=nearestObj)
                g.GetComponent<InventoryStat>().SetHalo(false);
        }
        return nearestObj;
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

            if (obj.GetComponent<InventoryStat>().AnaiObject)
            {
                TextUpdate(obj.GetComponent<InventoryStat>().Name + " is collected, " + health + " [health] were add to Anai");
                playerStat.AddHealth(health);
            }
            else if (obj.GetComponent<InventoryStat>().MimbiObject)
            {
                TextUpdate(obj.GetComponent<InventoryStat>().Name + " is collected, " + health + " [health] were add to Mimbi");
            }
            else
            {
                TextUpdate(obj.GetComponent<InventoryStat>().Name + " is collected. ");
            }
            //Add to inventory
            playerInventory.AddObj(obj.gameObject);
            //Destroy Gameobject after collect
            //obj.gameObject.SetActive(false);
            Destroy(obj.gameObject);
            //Play Pickup audio clip
            PlayPickup();
        }
    }
    void DelayMethod()
    {
        inventoryAdd.gameObject.SetActive(false);
    }
    private void PlayPickup()
    {
        audioManager.Play("pickup01");
    }

    public void TextUpdate(string s)
    {
        inventoryAdd.SetText(s);
    }

    private void CheckCurrentPlayer()
    {
        if (PlayerAnai.GetComponent<AnaiController>().Playing == true)
            CurrentPlayer = PlayerAnai;
        else
            CurrentPlayer = PlayerMimbi;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCurrentPlayer();
        GameObject closest = FindClosest();

        if (Vector3.Distance(FindClosest().transform.position, CurrentPlayer.transform.position) <= distanceToPickup)
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
            if(closest != null)
                closest.GetComponent<InventoryStat>().SetHalo(false);
        }

    }
}
