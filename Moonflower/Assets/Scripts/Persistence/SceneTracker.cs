using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker current;

    public new FollowCamera camera; // Reference created in Camera script
    public GameObject anai; // Reference created in Anai controller
    public GameObject mimbi; // Reference created in Mimbi controller
    public List<GameObject> npcs = new List<GameObject>(); // NPCs add themselves to this list

    private const int NPC_LIMIT = 50;

    //public List<GameObject> deadNPCs = new List<GameObject>(); 

    void Start()
    {
        if (current == null)
        {
            DontDestroyOnLoad(gameObject);
            current = this;
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }
    }

    public void RegisterNPC(GameObject npc)
    {
        if (npcs.Count < NPC_LIMIT)
            npcs.Add(npc);
        else
            Destroy(npc);
    }
    public void RegisterNPCDeath(GameObject npc)
    {
        npcs.Remove(npc);
        //deadNPCs.Add(npc);
    }

    public void SaveNPCs(out List<GameObject> aliveNPCs)
    {
        aliveNPCs = new List<GameObject>();

        foreach (GameObject npc in npcs)
        {
            //if (!deadNPCs.Contains(npc))
            //{
                aliveNPCs.Add(npc);
            //}
        }
    }
}
