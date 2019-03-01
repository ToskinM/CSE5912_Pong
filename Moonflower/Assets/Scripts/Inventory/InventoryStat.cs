using System.Collections;
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

    SkyColors.SkyCategory currentTime;



    // Start is called before the first frame update
    void Start()
    {
        currentTime = FindObjectOfType<SkyColors>().GetDayNight();
        if (currentTime ==null)
        {
            currentTime = SkyColors.SkyCategory.Sunset;
        }
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
