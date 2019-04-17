using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaiHouseItem : MonoBehaviour, IItems
{
    public static AnaiHouseItem instance;
    public GameObject WolfApple;
    public List<CollidableData> AnaiHouseCollidable;
    private SceneController scene;
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
        AnaiHouseCollidable = new List<CollidableData>();
        //AnaiHouseCollidable = new List<CollidableData>();
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        if (!scene.WentScene.Contains(Constants.SCENE_ANAIHOUSE))
        {
            ItemPosition();
        }
        AddItem();
    }

    public void ItemPosition()
    {
        AnaiHouseCollidable.Add(new CollidableData("WolfApple", WolfApple, new Vector3(0.7494713f, 1.2f, -58.01416f), Quaternion.Euler(0,-91.329f,0), new Vector3(5, 5, 5)));

    }
    public void AddItem()
    {
        //Debug.Log(VillageCollidable.Count);
        foreach (CollidableData collidable in AnaiHouseCollidable)
        {
            GameObject newObj = Instantiate(collidable.obj);
            newObj.name = collidable.name;
            newObj.transform.parent = gameObject.transform;
            newObj.transform.localScale = collidable.scale;
            newObj.transform.localPosition = collidable.position;
            newObj.transform.localRotation = collidable.rotation;
        }
    }
    public void RemoveItem(string ObjName, GameObject obj)
    {
        CollidableData collidableInList = AnaiHouseCollidable.Find(c => c.name == ObjName);
        //Debug.Log(VillageCollidable.Contains(collidableInList));
        AnaiHouseCollidable.Remove(collidableInList);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
