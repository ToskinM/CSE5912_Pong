using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCData : MonoBehaviour
{
    public List<GameObject> aliveNPCs;

    public NPCData()
    {
        SceneTracker.current.SaveNPCs(out aliveNPCs);
    }

    public void Load()
    {
        foreach (GameObject npc in aliveNPCs)
        {
            Instantiate(npc);
        }
    }
}
