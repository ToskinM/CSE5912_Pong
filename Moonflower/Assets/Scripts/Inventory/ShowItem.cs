using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowItem : MonoBehaviour
{
    public GameObject skyObj;
    public SkyColors skyColors;
    public SkyColors.SkyCategory currentTime;
    private GameObject[] allCollidable;
    private List<GameObject> NightOnlyCollidables = new List<GameObject>();
    private List<GameObject> DayOnlyCollidables = new List<GameObject>();
    private List<GameObject> AllDayCollidables = new List<GameObject>();

    private Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        //get all object that can be pickup
        allCollidable = GameObject.FindGameObjectsWithTag("Collectable");
        if (allCollidable!=null)
            DistinguishObject(allCollidable);
        scene = SceneManager.GetActiveScene();
    }

    private void DistinguishObject(GameObject[] allGameObject)
    {
        foreach (GameObject items in allCollidable)
        {
            InventoryStat.DayNightCateogry itemdayNightCateogry;
            itemdayNightCateogry = items.GetComponent<InventoryStat>().GetDayNightCategory();
            if (itemdayNightCateogry == InventoryStat.DayNightCateogry.Night)
                NightOnlyCollidables.Add(items);
            else if (itemdayNightCateogry == InventoryStat.DayNightCateogry.Day)
                DayOnlyCollidables.Add(items);
            else
                AllDayCollidables.Add(items);

        }
    }

    private void RefreshAllItems()
    {
        NightOnlyCollidables = new List<GameObject>();
        DayOnlyCollidables = new List<GameObject>();
        AllDayCollidables = new List<GameObject>();
        DistinguishObject(allCollidable);
        scene = SceneManager.GetActiveScene();
    }

    private void ChangeItemActive(List<GameObject> category, bool active )
    {
        if (category != null)
        {
            foreach (GameObject itemObj in category)
            {
                if (itemObj == null)
                {
                    category.Remove(itemObj);
                    break;
                }
                if (itemObj.activeInHierarchy !=active)
                    itemObj.SetActive(active);
            }
        }
    }

    private void CheckDayNightCycletoShowItems()
    {
        //Get Current Time
        currentTime = skyColors.GetDayNight();

        //Case Day time
        if (currentTime == SkyColors.SkyCategory.Day)
        {
            ChangeItemActive(DayOnlyCollidables, true);
            ChangeItemActive(NightOnlyCollidables, false);
        }
        //Case Sunset
        else if (currentTime == SkyColors.SkyCategory.Sunset )
        {
            ChangeItemActive(DayOnlyCollidables, true);
            ChangeItemActive(NightOnlyCollidables, true);
        }
        //Case Night
        else if (currentTime == SkyColors.SkyCategory.Night)
        {
            ChangeItemActive(DayOnlyCollidables, false);
            ChangeItemActive(NightOnlyCollidables, true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckDayNightCycletoShowItems();
        if (SceneManager.GetActiveScene() != scene)
            RefreshAllItems();
    }
}
