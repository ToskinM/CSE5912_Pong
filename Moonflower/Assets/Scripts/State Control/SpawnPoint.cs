using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPoint : MonoBehaviour
{
    public string thisScene;
    public string previousScene;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Spawn()
    {
        transform.position = GameObject.Find(previousScene + " Spawn").transform.position;          
        PlayerController.instance.GetActivePlayerObject().transform.position = this.transform.position;
        PlayerController.instance.GetCompanionObject().GetComponent<NavMeshAgent>().Warp(this.transform.position + new Vector3(0, 0, 2));

    }
    // Update is called once per frame
    void Update()
    {

    }
}
