﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    private AudioManager audioManager;
    private bool walk;
    private int mimbiStep;
    //private int footstep;
    //private int runStep;
    //public int footstepTime = 8;
    public int mimbiFootStepTime = 6;
    //public int runStepTime = 5;
    private readonly string anai = "Anai";
    private readonly string mimbi = "Mimbi";


    // Start is called before the first frame update
    void Start()
    {
        mimbiStep = 0;
        //footstep = 0;
        walk = false;
        StartCoroutine(GetAudioManager());
    }
    public IEnumerator GetAudioManager()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            yield return null;     }
    }
    public void AnaiAttackSFX()
    {
        audioManager.Play(anai,"AnaiAttackSwing");
    }
    public void AnaiWalkingSFX()
    {
        audioManager.ResumeNormal(anai,"AnaiWalking(1)");
        audioManager.ResumeNormal(anai, "AnaiWalking(2)");
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
                audioManager.Play(anai, "AnaiWalking(1)");
            else
                audioManager.Play(anai, "AnaiWalking(2)");
        }

    }
    public void AnaiSneakingSFX()
    {
        int x = Random.Range(1, 3);
        if (x == 1)
            audioManager.PlaySneakFootStep(anai, "AnaiWalking(1)");
        else
            audioManager.PlaySneakFootStep(anai, "AnaiWalking(2)");
    }

    public void AnaiPunchSFX()
    {
        audioManager.Play(anai, "AnaiPunch");
    }

    public void AnaiKickSFX()
    {
        audioManager.Play(anai, "AnaiKick");
    }

    public void AnaiRunSFX()
    {
        audioManager.Play(anai, "AnaiRun");

    }
    public void PlayerPickupSFX()
    {
        audioManager.Play("Player", "PlayerPickup");
    }

    public void MimbiAttackSFX()
    {
        audioManager.Play(mimbi,"MimbiAttack");
    }

    public void MimbiWalkingSFX()
    {
        if (mimbiStep == 0)
        {
            audioManager.Play(mimbi,"MimbiWalking");
        }
        mimbiStep++;
        if (mimbiStep == mimbiFootStepTime)
            mimbiStep = 0;
    }

    public void AnaiMute()
    {
        audioManager.MuteCategoryVol(anai);
    }
    public void AnaiResume()
    {
        audioManager.ResumeCategoryVol(anai);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
