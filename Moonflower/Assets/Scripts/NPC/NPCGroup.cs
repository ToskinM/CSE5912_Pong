using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGroup : MonoBehaviour
{
    public string groupName;
    public List<LesserNPCController> NPCs;

    public bool assistInCombat;
    public bool cantHurtEachother;
    public bool destroyIfEmpty;

    void Start()
    {
        // Remove any null refernces
        while (NPCs.Remove(null)) { }

        // Subscribe to events
        foreach (LesserNPCController npc in NPCs)
        {
            npc.combatController.group = this;
            npc.combatController.OnAggroUpdated += HandleAggroEvent;
            npc.combatController.OnDeath += HandleDeathEvent;
        }
    }

    public bool IsInGroup(GameObject npc)
    {
        foreach (LesserNPCController member in NPCs)
        {
            if (member.gameObject == npc)
            {
                return true;
            }
        }
        return false;
    }

    // Aggro the group if a member is attacked
    void HandleAggroEvent(bool allyAggroed, GameObject aggroTarget)
    {
        if (assistInCombat)
        {
            foreach (LesserNPCController npc in NPCs)
            {
                if (allyAggroed && !npc.combatController.InCombat)
                {
                    npc.combatController.Aggro(aggroTarget, false);
                }
            }
        }
    }
    
    // Remove killed npcs from the group
    void HandleDeathEvent(NPCCombatController npcCombatController)
    {
        LesserNPCController npcToRemove = null;

        // Find and remove the npc whose combatcontroller signaled they died
        foreach (LesserNPCController npc in NPCs)
            if (npc.combatController = npcCombatController)
            {
                npcToRemove = npc;
                break;
            }

        if (npcToRemove)
            NPCs.Remove(npcToRemove);

        if (destroyIfEmpty && NPCs.Count == 0)
            Destroy(gameObject);
    }

    // Draw editor lines
    void OnDrawGizmosSelected()
    {
        foreach (LesserNPCController npc in NPCs)
        {
            if (npc)
            {
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, npc.transform.position);
            }
        }
    }
}
