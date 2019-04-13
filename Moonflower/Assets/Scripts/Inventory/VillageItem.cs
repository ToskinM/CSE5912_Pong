using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageItem : MonoBehaviour
{
    public GameObject pumpkin;
    public GameObject SweetPotato;
    public GameObject Pineapple;
    public GameObject flute;
    public GameObject peanut;
    public GameObject paint;
    public GameObject bow;
    public GameObject staff;
    public GameObject corn;
    public GameObject pot;
    public GameObject arrow;
    public GameObject feather;
    public GameObject djembe;
    public GameObject necklace;
    public List<CollidableData> VillageCollidable;
    public List<CollidableData> VillageTest;


    SceneController scene;


    // Start is called before the first frame update
    void Start()
    {
        VillageCollidable = new List<CollidableData>();
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        if (!scene.WentScene.Contains(Constants.SCENE_VILLAGE))
        {
            ItemPosition();
        }
        AddItemInVillage();
    }
    public void ItemPosition()
    {
        VillageCollidable.Add(new CollidableData(pumpkin, new Vector3(298.23f, 107.9f, -71.6f), Quaternion.Euler(Vector3.zero), new Vector3(1,1,1)));
        VillageCollidable.Add(new CollidableData(pumpkin, new Vector3(296.8f, 107.9f, -69.85f), Quaternion.Euler(Vector3.zero), new Vector3(1, 1, 1)));
        VillageCollidable.Add(new CollidableData(SweetPotato, new Vector3(300.18f, 107.9f, -73.26f), Quaternion.Euler(Vector3.zero), new Vector3(0.3f, 0.3f, 0.3f)));
        VillageCollidable.Add(new CollidableData(Pineapple, new Vector3(333.92f, 107.64f, -37.73f), Quaternion.Euler(90,0,0), new Vector3(0.8f, 0.8f, 0.8f)));
        VillageCollidable.Add(new CollidableData(flute, new Vector3(310.57f, 106.962f, -12.14f),Quaternion.Euler(Vector3.zero), new Vector3(0.004f, 0.004f, 0.004f)));
        VillageCollidable.Add(new CollidableData(peanut, new Vector3(307.467f, 107.589f, -45.94f), Quaternion.Euler(0,90,0), new Vector3(1, 1, 1)));
        VillageCollidable.Add(new CollidableData(paint, new Vector3(317.252f, 107.509f, -9.558f), Quaternion.Euler(Vector3.zero), new Vector3(0.3f, 0.3f, 0.3f)));
        VillageCollidable.Add(new CollidableData(bow, new Vector3(296.75f, 108.71f, -19.042f), Quaternion.Euler(18.99f,-91.732f,0), new Vector3(100, 100, 100)));
        VillageCollidable.Add(new CollidableData(staff, new Vector3(296.580f, 108.93f, -18.472f), Quaternion.Euler(19.707f,-91.732f,0), new Vector3(90, 90, 90)));
        VillageCollidable.Add(new CollidableData(corn, new Vector3(188.35f, 107.9f, -64.89f), Quaternion.Euler(Vector3.zero), new Vector3(0.8f, 0.8f, 0.8f)));
        VillageCollidable.Add(new CollidableData(pot, new Vector3(317.327f, 107.58f, -7.569f), Quaternion.Euler(Vector3.zero), new Vector3(0.002f, 0.002f, 0.002f)));
        VillageCollidable.Add(new CollidableData(arrow, new Vector3(297.12f, 107.64f, -14.47f), Quaternion.Euler(90,0,91.732f), new Vector3(100f, 100f, 100f)));
        VillageCollidable.Add(new CollidableData(arrow, new Vector3(297.12f, 107.64f, -15.79f), Quaternion.Euler(90, 0, 91.732f), new Vector3(100f, 100f, 100f)));
        VillageCollidable.Add(new CollidableData(feather, new Vector3(299.866f, 107.622f, -25.37f), Quaternion.Euler(30.8f, 0, 0f), new Vector3(0.02f, 0.02f, 0.02f)));
        VillageCollidable.Add(new CollidableData(feather, new Vector3(299.904f, 107.622f, -25.464f), Quaternion.Euler(0.54f, 0, 0), new Vector3(0.02f, 0.02f, 0.02f)));
        VillageCollidable.Add(new CollidableData(djembe, new Vector3(308.85f, 107.96f, -12.87f), Quaternion.Euler(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
        VillageCollidable.Add(new CollidableData(necklace, new Vector3(333.82f, 109.12f, 7.52f), Quaternion.Euler(0, 0, 0), new Vector3(1f, 1f, 1f)));
        VillageCollidable.Add(new CollidableData(corn, new Vector3(289.06f, 107.9f, -63.37f), Quaternion.Euler(0, 0, 0), new Vector3(0.8f, 0.8f, 0.8f)));
    }

    public void AddTest(GameObject obj, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (VillageTest != null)
        {
            VillageTest.Add(new CollidableData(obj, position, rotation, scale));
            Debug.Log(VillageTest[0].ToString());
        }
    }

    public void RemoveTest(GameObject obj, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (obj.name.Contains("WolfApple") || obj.name.Contains("Moonflower"))
        {

        }
        else
        {
            VillageTest.Remove(new CollidableData(obj, position, rotation, scale));
        }
            
    }

    public void GetPrefab()
    {
        
    }

    public void AddItemInVillage()
    {
        foreach (CollidableData collidable in VillageCollidable)
        {
            GameObject newObj = Instantiate(collidable.obj);
            newObj.transform.parent = gameObject.transform;
            newObj.transform.localScale = collidable.scale;
            newObj.transform.localPosition = collidable.position;
            newObj.transform.localRotation = collidable.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
