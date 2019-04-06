using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonFlowerRegenerate : MonoBehaviour
{
    //private int MaxNoOfMoonflower = 5;
    public GameObject MoonFlower;
    public GameObject WolfApple;

    // Start is called before the first frame update
    void Start()
    {
        //WolfApple = Resources.Load("Assets/Prefabs/Items/Apple.prefab");

    }

    public void ReAddObj(Vector3 position, Quaternion rotation)
    {
        GameObject childObject = Instantiate(MoonFlower, position, rotation);
        childObject.transform.parent = gameObject.transform;
    }
    public void ReAddWolfApple(Vector3 position, Quaternion rotation)
    {
        GameObject childObject = Instantiate(WolfApple, position, rotation);//as GameObject;
        childObject.transform.parent = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
