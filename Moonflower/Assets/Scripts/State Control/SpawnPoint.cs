using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string thisScene;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Spawn()
    {
        if (thisScene.Equals("The Village"))
        {
            PlayerController.instance.AnaiObject.transform.position = this.transform.position;
            PlayerController.instance.MimbiObject.transform.position = this.transform.position + new Vector3(0, 2, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
