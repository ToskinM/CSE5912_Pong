using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    private AudioManager audioManager;
    private bool walk;
    private int footstep;
    private int runStep;
    public int footstepTime = 8;
    public int runStepTime = 5;

    // Start is called before the first frame update
    void Start()
    {
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
        if (footstep == 0)
        {
            audioManager.Play("AnaiWalking");
        }
        footstep++;
        if (footstep == footstepTime)
            footstep = 0;

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
        if (runStep == 0)
            audioManager.Play("AnaiRun");
        runStep++;
        if (runStep == runStepTime)
            runStep = 0;
    }
    public void PlayerPickupSFX()
    {
        audioManager.Play("PlayerPickup");
    }

    public void MimbiAttackSFX()
    {
        audioManager.Play("MimbiAttack");
    }

// Update is called once per frame
    void Update()
    {

    }
}
