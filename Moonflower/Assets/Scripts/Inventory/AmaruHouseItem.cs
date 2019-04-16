using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaruHouseItem : MonoBehaviour, IItems
{
    public static AmaruHouseItem instance;
    public GameObject pot;
    public GameObject peanut;
    public GameObject chipa;
    public List<CollidableData> AmaruHouseCollidable;
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
        AmaruHouseCollidable = new List<CollidableData>();
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        if (!scene.WentScene.Contains(Constants.SCENE_AMARUHOUSE))
        {
            ItemPosition();
        }
        AddItem();

    }

    public void ItemPosition()
    {
        AmaruHouseCollidable.Add(new CollidableData("chipa", chipa, new Vector3(-0.01038647f, 0.4379895f, 4.814713f), Quaternion.Euler(Vector3.zero), new Vector3(0.77107f, 0.77107f, 0.77107f)));
        AmaruHouseCollidable.Add(new CollidableData("peanut", peanut, new Vector3(-0.4097123f, 2.76212f, 1.705921f), Quaternion.Euler(Vector3.zero), new Vector3(0.8632078f, 0.8632078f, 0.8632078f)));
        AmaruHouseCollidable.Add(new CollidableData("pot", pot, new Vector3(-0.3303866f, -2.022011f, 3.124714f), Quaternion.Euler(Vector3.zero), new Vector3(0.002f, 0.002f, 0.002f)));

    }

    public void RemoveItem(string ObjName, GameObject obj)
    {
        CollidableData collidableInList = AmaruHouseCollidable.Find(c => c.name == ObjName);
        //Debug.Log(VillageCollidable.Contains(collidableInList));
        AmaruHouseCollidable.Remove(collidableInList);

    }

    public void AddItem()
    {
        //Debug.Log(NaiaHouseCollidable.Count);
        foreach (CollidableData collidable in AmaruHouseCollidable)
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
