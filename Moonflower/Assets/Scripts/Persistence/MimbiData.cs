using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MimbiData
{
    // Controller
    public MimbiController mimbiController;
    public bool playing;
    public NPCMovement movement;

    // Movement
    public PlayerMovement playerMovement;
    public Vector3 position;
    public Quaternion rotation;
    public Actions action;
    public bool jumping;
    public float jumpTimer;

    // Combat
    public PlayerCombatController combatController;
    public bool isBlocking;
    public bool hasWeaponOut;
    public bool inCombat;


    public MimbiData(GameObject mimbi)
    {
        mimbiController = mimbi.GetComponent<MimbiController>();
        playerMovement = mimbi.GetComponent<PlayerMovement>();
        combatController = mimbi.GetComponent<PlayerCombatController>();

        // Controller
        playing = mimbiController.Playing;

        // Movement
        position = mimbi.transform.position;
        rotation = mimbi.transform.rotation;
        action = playerMovement.Action;
        //jumping = anaiMovement.Jumping;
        //jumpTimer = anaiMovement.jumpTimer;

        hasWeaponOut = combatController.HasWeaponOut;
    }

    public void Load(GameObject mimbi)
    {
        mimbiController.Playing = playing;
        mimbiController.Switch(playing);

        mimbi.transform.position = position;
        mimbi.transform.rotation = rotation;
        playerMovement.Action = action;

        combatController.SetWeaponSheathed(!hasWeaponOut);
        combatController.ApplyLoad();
    }
}
