using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupArea : MonoBehaviour
{
    public CharacterStats Stats { get; private set; }
    public GameObject Player;
    private CharacterStats playerStat;
    //private InventoryManager inventoryManager;
    private PlayerInventory playerInventory;
    private AudioManager audioManager;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("background");
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
            //Get items's health
            int health = other.gameObject.GetComponent<InventoryStat>().GetHealth();
            //Destroy Gameobject after collect
            Destroy(other.gameObject);

            if (other.gameObject.GetComponent<InventoryStat>().AnaiObject)
            {
                Debug.Log(other.gameObject.tag + " is collected, " + health + " were add to anai");
                playerInventory.AddMoonFlower(other.gameObject);
                playerStat.AddHealth(health);
            }
            else
            {
                Debug.Log(other.gameObject.tag + " is collected, " + health + " were add to mimmbi");
                playerInventory.AddWolfApple(other.gameObject);
            }
            audioManager.Play("pickup01");
        }
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

