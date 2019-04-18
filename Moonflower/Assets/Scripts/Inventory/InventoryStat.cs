using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;

public class InventoryStat : MonoBehaviour
{
    public string Name;
    public int Strength;
    public int Attack;
    public int Defense;
    public int Health;
    public bool AnaiObject;
    public bool MimbiObject;
    public Sprite objectImage;

    public enum DayNightCateogry{Day, Night, AllDay}
    public DayNightCateogry DayNight;

    public enum InventoryCategory { consumable, key, material }
    public InventoryCategory inventoryCat;

    public Behaviour halo;

    //public SceneController sceneController;

    SkyColors.SkyCategory currentTime;
    public VillageItem villageItem;

    private bool outlined;

    void Start()
    {
        //sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        SkyColors skyColors = FindObjectOfType<SkyColors>();
        if (currentTime == null)
        {
            currentTime = SkyColors.SkyCategory.Sunset;
        }
        if (skyColors)
            currentTime = skyColors.GetDayNight();

        //villageItem = GameObject.Find("Item").GetComponent<VillageItem>();
        //villageItem.AddTest(gameObject, transform.position, transform.localRotation, transform.localScale);
        //Destroy(gameObject);


    }
    public void SetHalo(bool enableHalo)
    {
        //halo.enabled = enableHalo;

        // Outline effect instead
        if (enableHalo)
        {
            // apply outline if there isnt already one
            if (!outlined)
            {
                outlined = true;
                foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                {
                    renderer.gameObject.AddComponent<cakeslice.Outline>();
                }
            }
        }
        else
        {
            // remove outline if there is one
            if (outlined)
            {
                foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                {
                    Destroy(renderer.gameObject.GetComponent<cakeslice.Outline>());
                }
                outlined = false;
            }
        }

    }

    public int GetHealth ()
    {
        return Health;
    }
    public DayNightCateogry GetDayNightCategory()
    {
        return DayNight;
    }

    // Update is called once per frame
    void Update()
    {
    }

}
