﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public void SetHalo(bool decide)
    {
        halo.enabled = decide;
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
