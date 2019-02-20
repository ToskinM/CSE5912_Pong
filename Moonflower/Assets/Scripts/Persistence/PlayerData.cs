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
    public PlayerMovement anaiMovement;
    public Vector3 position;
    public Quaternion rotation;
    public Actions action;
    public bool jumping;
    public float jumpTimer;

    // Combat
    public PlayerCombatController anaiCombat;
    public bool isBlocking;
    public bool hasWeaponOut;
    public bool inCombat;


    public AnaiData(GameObject anai)
    {
        anaiController = anai.GetComponent<AnaiController>();
        anaiMovement = anai.GetComponent<PlayerMovement>();
        anaiCombat = anai.GetComponent<PlayerCombatController>();

        // Controller
        //playing = anaiController.Playing;

        // Movement
        position = anai.transform.position;
        rotation = anai.transform.rotation;
        action = anaiMovement.Action;
        //jumping = anaiMovement.Jumping;
        //jumpTimer = anaiMovement.jumpTimer;

        hasWeaponOut = anaiCombat.hasWeaponOut;
    }

    public void Load(GameObject anai)
    {
        anai.transform.position = position;
        anai.transform.rotation = rotation;
        anaiMovement.Action = action;

        anaiCombat.SetWeaponSheathed(!hasWeaponOut);
        anaiCombat.Load();
    }
}
