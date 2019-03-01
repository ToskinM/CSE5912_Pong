using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItem : MonoBehaviour
{
    public GameObject skyObj;
    public SkyColors.SkyCategory currentTime;
    private GameObject[] allCollidable;

    // Start is called before the first frame update
    void Start()
    {
        allCollidable = GameObject.FindGameObjectsWithTag("Collectable");
    }

    private void CheckDayNightCycletoShowItems()
    {
        //Get Current Time
        currentTime = FindObjectOfType<SkyColors>().GetDayNight();
        //Get All items
        foreach (GameObject collidableObj in allCollidable)
        {
            if (collidableObj == null)
            {
                break;
            }
            InventoryStat.DayNightCateogry dayNightCateogry;
            dayNightCateogry = collidableObj.GetComponent<InventoryStat>().GetDayNightCategory();
            if (dayNightCateogry == InventoryStat.DayNightCateogry.AllDay || currentTime == SkyColors.SkyCategory.Sunset)
            {
                collidableObj.SetActive(true);
            }
            else if (dayNightCateogry == InventoryStat.DayNightCateogry.Day)
            {
                if (currentTime == SkyColors.SkyCategory.Day)
                    collidableObj.SetActive(true);
                else
                    collidableObj.SetActive(false);
            }
            else if (dayNightCateogry == InventoryStat.DayNightCateogry.Night)
            {
                if (currentTime == SkyColors.SkyCategory.Night)
                    collidableObj.SetActive(true);
                else
                {
                    collidableObj.SetActive(false);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckDayNightCycletoShowItems();
    }
}
