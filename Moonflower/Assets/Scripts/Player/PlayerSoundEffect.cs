using System.Collections;
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
    public void PlayAttackSFX()
    {
        audioManager.Play("AttackSwing");
    }
    public void PlayWalkingSFX()
    {
        audioManager.Play("AnaiWalking");
    }

    public void PlayPunchSound01()
    {
        audioManager.Play("AnaiPunch");
    }

    public void PlayKickSound01()
    {
        audioManager.Play("AnaiKick");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
