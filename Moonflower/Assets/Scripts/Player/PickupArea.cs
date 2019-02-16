using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupArea : MonoBehaviour
{
    public CharacterStats Stats { get; private set; }
    public GameObject Player;
    private CharacterStats playerStat;
    //private InventoryManager inventoryManager;
    private PlayerInventory playerInventory;
    private AudioManager audioManager;

    private List<GameObject> currentCollisions = new List<GameObject>();
    private GameObject[] allCollidable;
    public TextMeshProUGUI inventoryAdd;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            inventoryAdd.gameObject.SetActive(true);
            Invoke("DelayMethod", 2f);
            //Get closest items
            GameObject obj = FindClosest();
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
 
    private void PlayPickup()
    {
        audioManager.Play("pickup01");
    }

    public void textUpdate(string s)
    {
        inventoryAdd.SetText(s);
    }
    void DelayMethod()
    {
        inventoryAdd.gameObject.SetActive(false);
    }
    private GameObject FindClosest()
    {
        allCollidable = GameObject.FindGameObjectsWithTag("Collectable");
        GameObject nearestObj = allCollidable[0];
        float nearest = Vector3.Distance(allCollidable[0].transform.position,transform.position);
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
    // Update is called once per frame
    void Update()
    {

    }

}







//void RaycastCollision()
//{
//    RaycastHit hit;
//    //collision detection with obstacle

//    for (int i = 0; i < 90; i+=10)
//    {
//        for (int j = -90; j < 90; j+=10)
//        {
//            //Vector3 worldDirection = Quaternion.Euler(localEulerAngles) * transform.forward;
//            //Quaternion angle = Quaternion.Euler(i, j, 0);

//            Quaternion spreadAngleX = Quaternion.AngleAxis(i, new Vector3(1, 0, 0));
//            Quaternion spreadAngleY = Quaternion.AngleAxis(j, new Vector3(0, 1, 0));
//            Quaternion QuaResult = spreadAngleX * spreadAngleY;


//            Quaternion angle = Quaternion.Euler(i, j, 0);
//            Vector3 newVector = angle * transform.forward;
//            //draw the ray 
//            Debug.DrawRay(transform.position, newVector * 2, Color.black);


//            if (Physics.Raycast(transform.position, transform.TransformDirection(newVector), out hit, 2f))
//            {
//                Debug.Log(hit.collider.gameObject.name);
//            }
//        }
//    }

//}

