using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            PlayerController.instance.GetActivePlayerObject().transform.position = this.transform.position;
            PlayerController.instance.GetCompanionObject().GetComponent<NavMeshAgent>().Warp(this.transform.position + new Vector3(0, 0, 2));
        } else if(thisScene.Contains("House"))
        {
            PlayerController.instance.GetActivePlayerObject().transform.position = new Vector3(2, 2, -67);
            PlayerController.instance.GetCompanionObject().GetComponent<NavMeshAgent>().Warp(this.transform.position + new Vector3(0, 0, 1));
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
