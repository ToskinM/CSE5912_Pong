using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaiaHouseItem : MonoBehaviour, IItems
{
    public static NaiaHouseItem instance;
    public GameObject pot;
    public List<CollidableData> NaiaHouseCollidable;
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
        NaiaHouseCollidable = new List<CollidableData>();
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        if (!scene.WentScene.Contains(Constants.SCENE_NAIAHOUSE))
        {
            ItemPosition();
        }
        AddItem();

    }

    public void ItemPosition()
    {
        NaiaHouseCollidable.Add(new CollidableData("pot", pot, new Vector3(-4.02f, 2.39f, -64.82f), Quaternion.Euler(Vector3.zero), new Vector3(0.00127742f, 0.00127742f, 0.00127742f)));

    }

    public void RemoveItem(string ObjName, GameObject obj)
    {
        CollidableData collidableInList = NaiaHouseCollidable.Find(c => c.name == ObjName);
        //Debug.Log(VillageCollidable.Contains(collidableInList));
        NaiaHouseCollidable.Remove(collidableInList);

    }

    public void AddItem()
    {
        //Debug.Log(NaiaHouseCollidable.Count);
        foreach (CollidableData collidable in NaiaHouseCollidable)
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
