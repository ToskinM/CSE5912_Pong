using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFlower : MonoBehaviour
{
    public GameObject FlowerHolder;
    public GameObject Flower;
    public int noFlower = 3;
    private float firstPosition;
    private GameObject instantiateObject;
    private float interval = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        Flower.transform.localScale = new Vector3(8,8,8);
        MakeFlower(Flower);
    }
    //temp solution
    public void MakeFlower(GameObject obj)
    {
        FlowerHolder = new GameObject();
        FlowerHolder.name = obj.name;
        firstPosition = -75f;
        for (int i = 0; i < noFlower; i++)
        {
            instantiateObject = Instantiate(Flower, new Vector3(firstPosition, -0.55f, 0), Quaternion.identity);
            instantiateObject.transform.parent = FlowerHolder.transform;
            for (int j = -70; j < -53; j++)
            {
                instantiateObject = Instantiate(Flower, new Vector3(firstPosition, -0.55f, j), Quaternion.identity);
                instantiateObject.transform.parent = FlowerHolder.transform;
            }
            firstPosition += interval;
            //Destroy(ball, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
