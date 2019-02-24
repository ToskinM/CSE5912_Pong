﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    private AudioManager audioManager;
    private bool walk;
    // Start is called before the first frame update
    void Start()
    {
        walk = false;
        StartCoroutine(GetAudioManager());
    }
    public IEnumerator GetAudioManager()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            yield return null;
        }
    }
    public void AnaiAttackSFX()
    {
        audioManager.Play("AnaiAttackSwing");
    }
    public void AnaiWalkingSFX()
    {
        audioManager.Play("AnaiWalking");
    }

    public void AnaiPunchSFX()
    {
        audioManager.Play("AnaiPunch");
    }

    public void AnaiKickSFX()
    {
        audioManager.Play("AnaiKick");
    }
    
    public void PlayerPickupSFX()
    {
        audioManager.Play("PlayerPickup");
    }
    
// Update is called once per frame
    void Update()
    {

    }
}
