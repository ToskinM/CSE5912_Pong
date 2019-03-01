using System.Collections;
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
        audioManager.ResumeNormal(anai,"AnaiWalking");
        audioManager.Play(anai, "AnaiWalking");
    }
    public void AnaiSneakingSFX()
    {
        audioManager.PlaySneakFootStep(anai,"AnaiWalking");
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
