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
    public GameObject Player;
    private CharacterStats playerStat;
    //private InventoryManager inventoryManager;
    private PlayerInventory playerInventory;
    private AudioManager audioManager;

    public TextMeshProUGUI inventoryAdd;

    //Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetAudioManager());
        //inventoryManager = FindObjectOfType<InventoryManager>();
        playerStat = Player.GetComponent<CharacterStats>();
        playerInventory = Player.GetComponent<PlayerInventory>();
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
        float nearest = Vector3.Distance(allCollidable[0].transform.position, transform.position);
        foreach (GameObject g in allCollidable)
        {
            if (Vector3.Distance(g.transform.position, transform.position) < nearest)
            {
                nearest = Vector3.Distance(g.transform.position, transform.position);
                nearestObj = g;
            }
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
                textUpdate(obj.GetComponent<InventoryStat>().Name + " is collected, " + health + " [health] were add to Anai");
                playerStat.AddHealth(health);
            }
            else if (obj.GetComponent<InventoryStat>().MimbiObject)
            {
                textUpdate(obj.GetComponent<InventoryStat>().Name + " is collected, " + health + " [health] were add to Mimbi");
            }
            //Add to inventory
            playerInventory.AddObj(obj.gameObject);
            //Destroy Gameobject after collect
            obj.gameObject.SetActive(false);
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

    public void textUpdate(string s)
    {
        inventoryAdd.SetText(s);
    }



    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(FindClosest().transform.position, transform.position) <= distanceToPickup)
        {
            FindClosest().GetComponent<InventoryStat>().SetHalo(true);
            if (Input.GetButtonDown("Pickup"))
            {
                Debug.Log("I need to pickup");
                DoPickup(FindClosest());
            }

        }
        else
        {
            FindClosest().GetComponent<InventoryStat>().SetHalo(false);
        }

    }
}
