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
            LevelManager.current.anai.transform.position = this.transform.position;
            LevelManager.current.mimbi.transform.position = this.transform.position + new Vector3(0, 2, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
