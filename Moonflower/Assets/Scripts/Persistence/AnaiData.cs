using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnaiData
{
    // Controller
    public AnaiController anaiController;
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


    public AnaiData(GameObject anai)
    {
        anaiController = anai.GetComponent<AnaiController>();
        playerMovement = anai.GetComponent<PlayerMovement>();
        combatController = anai.GetComponent<PlayerCombatController>();

        // Controller
        playing = anaiController.Playing;

        // Movement
        position = anai.transform.position;
        rotation = anai.transform.rotation;
        action = playerMovement.Action;
        //jumping = anaiMovement.Jumping;
        //jumpTimer = anaiMovement.jumpTimer;

        hasWeaponOut = combatController.HasWeaponOut;
    }

    public void Load(GameObject anai)
    {
        anaiController = anai.GetComponent<AnaiController>();
        playerMovement = anai.GetComponent<PlayerMovement>();
        combatController = anai.GetComponent<PlayerCombatController>();

        anaiController.Playing = playing;
        anaiController.Switch(playing);

        anai.transform.position = position;
        anai.transform.rotation = rotation;
        playerMovement.Action = action;

        combatController.SetWeaponSheathed(!hasWeaponOut);
        combatController.ApplyLoad();
    }
}
