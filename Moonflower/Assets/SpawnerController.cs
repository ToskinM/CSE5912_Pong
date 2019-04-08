using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public NPCSpawner[] spawners;

    void Start()
    {
        
    }

    public void ActivateAll()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Activate();
        }
    }
    public void DeactivateAll()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Deactivate();
        }
    }
}
