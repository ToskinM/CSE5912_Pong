using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCData : MonoBehaviour
{
    public List<int> aliveNPCs;
    public Dictionary<string, DialogueTrigger> NPCDialogues;

    public NPCData()
    {
        LevelManager.current.SaveNPCs(out aliveNPCs);
        NPCDialogues = new Dictionary<string, DialogueTrigger>(); 
    }

    public void Load()
    {
        LevelManager.current.LoadNPCs(aliveNPCs);
    }
}

public class NPCTransformInfo
{
    public Vector3 position;
    public Quaternion rotation;

    public NPCTransformInfo(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    public void SetInfo(Vector3 position, Quaternion rotation)
    {
        if (position != null)
            this.position = position;
        if (rotation != null)
            this.rotation = rotation;
    }
}


