using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcsToSpawn;
    public NPCGroup group;
    public float spawnInterval = 3f;
    public int maxNumberToSpawn = 0;
    public int maxAliveAtOnce = 2;

    public bool active = true;

    private float timeSinceLastSpawn = 0f;
    public List<GameObject> spawns;
    private int numSpawned;
    private bool spawning;

    void Start()
    {
        spawns = new List<GameObject>();
    }

    void Update()
    {
        if (!spawning)
            timeSinceLastSpawn += Time.deltaTime;

        if (active)
        {
            if (timeSinceLastSpawn >= spawnInterval)
            {
                if (maxNumberToSpawn == 0 || numSpawned < maxNumberToSpawn)
                {
                    if (spawns.Count < maxAliveAtOnce)
                    {
                        StartCoroutine(SpawnNPC());
                    }
                }
            }
        }
    }

    public void Activate()
    {
        active = true;
    }
    public void Deactivate()
    {
        active = false;
    }

    private IEnumerator SpawnNPC()
    {
        spawning = true;
        timeSinceLastSpawn = 0f;

        yield return new WaitForSeconds(1f);

        GameObject spawn = Instantiate(npcsToSpawn[Random.Range(0, npcsToSpawn.Length)], transform.position + new Vector3(Random.Range(-2, 2), 0f, Random.Range(-2, 2)), transform.rotation, gameObject.transform);
        LesserNPCController spawnController = spawn.GetComponent<LesserNPCController>();
        spawns.Add(spawn);
        spawnController.combatController.OnDeath += HandleDeathEvent;

        if (group != null)
        {
            group.Add(spawnController);
        }

        numSpawned++;

        spawning = false;
    }

    // Remove killed npcs from the spawns list
    void HandleDeathEvent(ICombatController npcCombatController)
    {
        GameObject npcToRemove = null;

        // Find and remove the npc whose combatcontroller signaled they died
        foreach (GameObject npc in spawns)
            if (npc == npcCombatController.GameObject)
            {
                npcToRemove = npc;
                break;
            }

        if (npcToRemove)
            spawns.Remove(npcToRemove);
    }
}
