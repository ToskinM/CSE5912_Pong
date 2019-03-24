using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current = null;

    [Header("Controllers")]
    public GameStateController gameStateController;
    public AudioManager audioController;
    public GameObject dummyHUD;

    [Header("Cameras")]
    public new FollowCamera mainCamera; // Reference created in Camera script
    public new DialogueCamera dialogueCamera; // Reference created in Camera script

    [Header("Player   (These should be set in the editor!)")]
    public GameObject anai; 
    public GameObject mimbi; 
    public GameObject currentPlayer;

    public PlayerController player; // Reference created in Anai controller

    [Header("NPCs")]
    public List<GameObject> npcs = new List<GameObject>(); // NPCs add themselves to this list
    public List<int> deadNPCs = new List<int>(); // NPCs add themselves to this list
    public Dictionary<int, NPCTransformInfo> npcsD = new Dictionary<int, NPCTransformInfo>(); // NPCs add themselves to this list

    private const int NPC_LIMIT = 50;

    //public List<GameObject> deadNPCs = new List<GameObject>(); 

    void Awake()
    {
        if (!current)
        {
            //DontDestroyOnLoad(gameObject);
            current = this;
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }

        currentPlayer = anai;

        npcs.Clear();
        npcsD.Clear();
        deadNPCs.Clear();
    }

    private void Start()
    {
        //yield return new WaitForSeconds(1);
        //DataSavingManager.current.npcData?.Load();
    }

    public void RequestDialogueCamera(GameObject dialoguePartner)
    {
        gameStateController.SetMouseLock(false);
        dialogueCamera.Enter(dialoguePartner.transform, mainCamera.transform);
        mainCamera.SetRendering(false);
    }
    public void ReturnDialogueCamera()
    {
        gameStateController.SetMouseLock(true);
        dialogueCamera.StartExit(mainCamera.transform);
    }

    public void RegisterNPC(GameObject npc)
    {
        if (npcs.Count < NPC_LIMIT)
        {
            npcs.Add(npc);
            //npcsD.Add(npcs.IndexOf(npc), new NPCTransformInfo(npc.transform));
        }
        else
            Destroy(npc);
    }
    public void RegisterNPCDeath(GameObject npc)
    {
        int index = npcs.IndexOf(npc);

        //npcs.Remove(npc);
        if (!deadNPCs.Contains(index))
            deadNPCs.Add(index);
    }

    public void SaveNPCs(out List<int> NPCs)
    {
        NPCs = new List<int>();

        foreach (int npc in deadNPCs)
        {
            //if (!deadNPCs.Contains(npc))
            //{
            NPCs.Add(npc);
            //}
        }
    }

    public void LoadNPCs(List<int> deadNPCs)
    {
        //foreach (KeyValuePair<int, NPCTransformInfo> npc in npcsD)
        //{
        //    int index = npc.Key;

        //    //go.transform.SetPositionAndRotation(npc.Value.position, npc.Value.rotation);

        //    if (!deadNPCs.Contains(go))
        //    {
        //        npc.GetComponent<NPCCombatController>().Kill();
        //    }
        //}

        foreach (int index in deadNPCs)
        {
            npcs[index].GetComponent<NPCCombatController>().Kill();
        }
    }


}

