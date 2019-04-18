using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickup : MonoBehaviour
{
    private List<GameObject> currentCollisions = new List<GameObject>();
    private GameObject[] allCollidable;
    public int distanceToPickup = 5;

    public CharacterStats Stats { get; private set; }

    private PlayerSoundEffect soundEffect;
    private InteractionPopup interaction;
    private FeedbackText feedback;

    public IItems sceneItem;
    public IItems DummyItem;


    //public TextMeshProUGUI inventoryAdd;

    void Start()
    {
        //StartCoroutine(GetAudioManager());
        //inventoryManager = FindObjectOfType<InventoryManager>();

        soundEffect = PlayerController.instance.GetActivePlayerObject().GetComponent<PlayerSoundEffect>();
        interaction = GameObject.Find("HUD").GetComponent<ShowInspect>().interaction;

        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
    }

    void OnEnable()
    {
        PlayerController.OnCharacterSwitch += SwitchActiveCharacter;
    }

    void OnDisable()
    {
        PlayerController.OnCharacterSwitch -= SwitchActiveCharacter;
    }

    private GameObject FindClosest()
    {
        allCollidable = GameObject.FindGameObjectsWithTag("Collectable");

        if (allCollidable.Length > 0)
        {
            GameObject nearestObj = allCollidable[0];
            float nearest = Vector3.Distance(allCollidable[0].transform.position, PlayerController.instance.GetActivePlayerObject().transform.position);
            foreach (GameObject g in allCollidable)
            {
                if (Vector3.Distance(g.transform.position, PlayerController.instance.GetActivePlayerObject().transform.position) < nearest)
                {
                    nearest = Vector3.Distance(g.transform.position, PlayerController.instance.GetActivePlayerObject().transform.position);
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
            float dist = Vector3.Distance(closest.transform.position, PlayerController.instance.GetActivePlayerObject().transform.position);
            if (dist <= distanceToPickup && !interaction.NotAllowed)
            {
                if (closest != null)
                {
                    InventoryStat stat = closest.GetComponent<InventoryStat>();
                    //if (!(stat.AnaiObject && !PlayerController.instance.AnaiIsActive()) && !(stat.MimbiObject && PlayerController.instance.AnaiIsActive()))
                    {
                        closest.GetComponent<InventoryStat>().SetHalo(true);
                        interaction.EnableItem(dist, stat.Name);
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
        //bool anaiObjectMatch = stat.AnaiObject && PlayerController.instance.AnaiIsActive(); //(currentPlayer.Equals(PlayerController.instance.AnaiObject));
        //bool mimbiObjectMatch = stat.MimbiObject && !PlayerController.instance.AnaiIsActive();

        //if (anaiObjectMatch || mimbiObjectMatch)
        {
            //Add text update
            //inventoryAdd.gameObject.SetActive(true);
            //Remove the text update
            Invoke("DelayMethod", 2f);
            string objName = obj.GetComponent<InventoryStat>().Name;
            feedback.ShowText("You have found a " + objName.ToLower());
            objectUsedImmediately = PlayerController.instance.ActivePlayerStats.AddHealth(health);

            //Add to inventory
            if (!objectUsedImmediately)
                PlayerController.instance.ActivePlayerInventory.AddObj(obj.gameObject);
            if (SceneManager.GetActiveScene().name == Constants.SCENE_ANAIHOUSE)
            {
                FindScene();
                sceneItem.RemoveItem(obj.name, obj.gameObject);
            }
            else if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE || SceneManager.GetActiveScene().name == Constants.SCENE_NAIAHOUSE || SceneManager.GetActiveScene().name == Constants.SCENE_AMARUHOUSE)
            {
                FindScene();
                sceneItem.RemoveItem(obj.name, obj.gameObject);
            }

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
                string textToShow = "You have found a " + objName.ToLower() + ".";
                if (!feedback.IsRepeat(textToShow))
                    feedback.ShowText(textToShow);

                if (SceneManager.GetActiveScene().name == Constants.SCENE_ANAIHOUSE)
                {
                    FindScene();
                    sceneItem.RemoveItem(obj.name, obj.gameObject);
                }
                else if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE || SceneManager.GetActiveScene().name == Constants.SCENE_NAIAHOUSE || SceneManager.GetActiveScene().name == Constants.SCENE_AMARUHOUSE)
                {
                    FindScene();
                    sceneItem.RemoveItem(obj.name,obj.gameObject);
                }
                //Add to inventory
                if(obj.gameObject != null)
                    PlayerController.instance.ActivePlayerInventory.AddObj(obj.gameObject);
                //else
                    //PlayerController.instance.ActivePlayerInventory.AddObj(objName);
                //Destroy Gameobject after collect
                Destroy(obj.gameObject);
                //Play Pickup audio clip;
                soundEffect.PlayerPickupSFX();
            }

        }
    }
    public void FindScene()
    {
        if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE)
            sceneItem = GameObject.Find("VillageItem").GetComponent<VillageItem>();
        else if (SceneManager.GetActiveScene().name == Constants.SCENE_NAIAHOUSE)
            sceneItem = GameObject.Find("NaiaHouseItems").GetComponent<NaiaHouseItem>();
        else if (SceneManager.GetActiveScene().name == Constants.SCENE_AMARUHOUSE)
            sceneItem = GameObject.Find("AmaruHouseItem").GetComponent<AmaruHouseItem>();
        else if (SceneManager.GetActiveScene().name == Constants.SCENE_ANAIHOUSE)
            sceneItem = GameObject.Find("AnaiHouseItems").GetComponent<AnaiHouseItem>();


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
        //currentPlayer = PlayerController.instance.GetActivePlayerObject();
    }
}
