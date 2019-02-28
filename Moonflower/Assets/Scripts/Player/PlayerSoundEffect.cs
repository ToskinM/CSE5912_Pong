using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    private AudioManager audioManager;
    private bool walk;
    private int mimbiStep;
    private int footstep;
    private int runStep;
    public int footstepTime = 8;
    public int mimbiFootStepTime = 6;
    public int runStepTime = 5;


    // Start is called before the first frame update
    void Start()
    {
        mimbiStep = 0;
        footstep = 0;
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
        audioManager.Play("AnaiAttackSwing");
    }
    public void AnaiWalkingSFX()
    {
        audioManager.Play("AnaiWalking");
    }
    public void AnaiSneakingSFX()
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

    public void AnaiRunSFX()
    {
        audioManager.Play("AnaiRun");

    }
    public void PlayerPickupSFX()
    {
        audioManager.Play("PlayerPickup");
    }

    public void MimbiAttackSFX()
    {
        audioManager.Play("MimbiAttack");
    }

    public void MimbiWalkSFX()
    {
        if (mimbiStep == 0)
        {
            audioManager.Play("MimbiWalk");
        }
        mimbiStep++;
        if (mimbiStep == mimbiFootStepTime)
            mimbiStep = 0;
    }

// Update is called once per frame
    void Update()
    {

    }
}
