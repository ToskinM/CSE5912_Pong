using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageItem : MonoBehaviour, IItems
{
    public static VillageItem instance;
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
    public GameObject apple;
    public GameObject moonflower;
    public List<CollidableData> VillageCollidable;
    //public List<CollidableData> AnaiHouseCollidable;
    SceneController scene;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        VillageCollidable = new List<CollidableData>();
        //AnaiHouseCollidable = new List<CollidableData>();
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        if (!scene.WentScene.Contains(Constants.SCENE_VILLAGE))
        {
            ItemPosition();
        }
        AddItem();
    }

    public void ItemPosition()
    {
        VillageCollidable.Add(new CollidableData("pumpkin(1)", pumpkin, new Vector3(298.23f, 107.9f, -71.6f), Quaternion.Euler(Vector3.zero), new Vector3(1,1,1)));
        VillageCollidable.Add(new CollidableData("pumpkin(2)", pumpkin, new Vector3(296.8f, 107.9f, -69.85f), Quaternion.Euler(Vector3.zero), new Vector3(1, 1, 1)));
        VillageCollidable.Add(new CollidableData("SweetPotato", SweetPotato, new Vector3(300.18f, 107.9f, -73.26f), Quaternion.Euler(Vector3.zero), new Vector3(0.3f, 0.3f, 0.3f)));
        VillageCollidable.Add(new CollidableData("Pineapple", Pineapple, new Vector3(333.92f, 107.64f, -37.73f), Quaternion.Euler(90,0,0), new Vector3(0.8f, 0.8f, 0.8f)));
        VillageCollidable.Add(new CollidableData("flute", flute, new Vector3(310.57f, 106.962f, -12.14f),Quaternion.Euler(Vector3.zero), new Vector3(0.004f, 0.004f, 0.004f)));
        VillageCollidable.Add(new CollidableData("peanut", peanut, new Vector3(307.467f, 107.589f, -45.94f), Quaternion.Euler(0,90,0), new Vector3(1, 1, 1)));
        VillageCollidable.Add(new CollidableData("paint", paint, new Vector3(317.252f, 107.509f, -9.558f), Quaternion.Euler(Vector3.zero), new Vector3(0.3f, 0.3f, 0.3f)));
        VillageCollidable.Add(new CollidableData("bow", bow, new Vector3(296.75f, 108.71f, -19.042f), Quaternion.Euler(18.99f,-91.732f,0), new Vector3(100, 100, 100)));
        VillageCollidable.Add(new CollidableData("staff", staff, new Vector3(296.580f, 108.93f, -18.472f), Quaternion.Euler(19.707f,-91.732f,0), new Vector3(90, 90, 90)));
        VillageCollidable.Add(new CollidableData("corn(1)", corn, new Vector3(188.35f, 107.9f, -64.89f), Quaternion.Euler(Vector3.zero), new Vector3(0.8f, 0.8f, 0.8f)));
        VillageCollidable.Add(new CollidableData("pot", pot, new Vector3(317.327f, 107.58f, -7.569f), Quaternion.Euler(Vector3.zero), new Vector3(0.002f, 0.002f, 0.002f)));
        VillageCollidable.Add(new CollidableData("arrow(1)", arrow, new Vector3(297.12f, 107.64f, -14.47f), Quaternion.Euler(90,0,91.732f), new Vector3(100f, 100f, 100f)));
        VillageCollidable.Add(new CollidableData("arrow(2)", arrow, new Vector3(297.12f, 107.64f, -15.79f), Quaternion.Euler(90, 0, 91.732f), new Vector3(100f, 100f, 100f)));
        VillageCollidable.Add(new CollidableData("feather(1)", feather, new Vector3(299.866f, 107.622f, -25.37f), Quaternion.Euler(30.8f, 0, 0f), new Vector3(0.02f, 0.02f, 0.02f)));
        VillageCollidable.Add(new CollidableData("feather(2)", feather, new Vector3(299.904f, 107.622f, -25.464f), Quaternion.Euler(0.54f, 0, 0), new Vector3(0.02f, 0.02f, 0.02f)));
        VillageCollidable.Add(new CollidableData("djembe", djembe, new Vector3(308.85f, 107.96f, -12.87f), Quaternion.Euler(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
        VillageCollidable.Add(new CollidableData("necklace", necklace, new Vector3(333.82f, 109.12f, 7.52f), Quaternion.Euler(0, 0, 0), new Vector3(1f, 1f, 1f)));
        VillageCollidable.Add(new CollidableData("corn(2)", corn, new Vector3(289.06f, 107.9f, -63.37f), Quaternion.Euler(0, 0, 0), new Vector3(0.8f, 0.8f, 0.8f)));
        VillageCollidable.Add(new CollidableData("WolfApple(1)", apple, new Vector3(317.3731f, 107.998f, -32.36111f), Quaternion.Euler(0, 90, 0), new Vector3(5f, 5f, 5f)));
        VillageCollidable.Add(new CollidableData("WolfApple(2)", apple, new Vector3(331.3874f, 108.348f, 11.63889f), Quaternion.Euler(0, 0, 0), new Vector3(5f, 5f, 5f)));
        VillageCollidable.Add(new CollidableData("MoonFlower(1)", moonflower, new Vector3(324.2096f, 109.1266f, -47.09322f), Quaternion.Euler(15.723f, -56.789f, -41.044f), new Vector3(0.002069252f, 0.002060364f, 0.002017667f)));
        VillageCollidable.Add(new CollidableData("MoonFlower(2)", moonflower, new Vector3(320.8814f, 108.6644f, -45.22709f), Quaternion.Euler(67.079f, -121.376f, -97.244f), new Vector3(0.002061143f, 0.002022629f, 0.002063513f)));
        VillageCollidable.Add(new CollidableData("MoonFlower(3)", moonflower, new Vector3(321.5826f, 109.462f, -48.58804f), Quaternion.Euler(58.123f, 3.28f, 42.756f), new Vector3(0.00206214f, 0.002023454f, 0.002061689f)));
        VillageCollidable.Add(new CollidableData("MoonFlower(4)", moonflower, new Vector3(322.8831f, 109.4775f, -49.6236f), Quaternion.Euler(27.569f, -16.354f, 21.965f), new Vector3(0.002058406f, 0.002052124f, 0.002036752f)));
        VillageCollidable.Add(new CollidableData("MoonFlower(5)", moonflower, new Vector3(320.2758f, 109.1944f, -46.70151f), Quaternion.Euler(43.358f, 43.52f, 93.03101f), new Vector3(0.00206394f, 0.002013957f, 0.002069385f)));
    }

    public void RemoveItem(string ObjName,GameObject obj)
    {
        CollidableData collidableInList = VillageCollidable.Find(c => c.name == ObjName);
        //Debug.Log(VillageCollidable.Contains(collidableInList));
        VillageCollidable.Remove(collidableInList);
    }

    public void RemoveItem(List<CollidableData> list, string name,GameObject obj, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (list == VillageCollidable)
            VillageCollidable.Remove(new CollidableData(name,obj, position, rotation, scale));
        //else if (list == AnaiHouseCollidable)
            //AnaiHouseCollidable.Remove(new CollidableData(name,obj, position, rotation, scale));

    }

    public void GetPrefab()
    {
        
    }

    public void AddItem()
    {
        //Debug.Log(VillageCollidable.Count);
        foreach (CollidableData collidable in VillageCollidable)
        {
            GameObject newObj = Instantiate(collidable.obj);
            newObj.name = collidable.name;
            newObj.transform.parent = gameObject.transform;
            newObj.transform.localScale = collidable.scale;
            newObj.transform.localPosition = collidable.position;
            newObj.transform.localRotation = collidable.rotation;
        }
    }


}
