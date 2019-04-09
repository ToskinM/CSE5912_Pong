using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvWeaponImpactSound : MonoBehaviour, IAudio
{
    public AudioClip[] ImpactSoundsList;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        SetupAudioSource();

        audioSource.volume = 0.25f;
    }

    public void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        AudioManager.OnSFXVolChange += OnVolumeChange;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHurtbox")
        {
            audioSource.clip = ImpactSoundsList[Random.Range(0, ImpactSoundsList.Length - 1)];
            audioSource.Play();
        }
    }

    public void OnVolumeChange(float volume)
    {
        audioSource.volume = volume * 0.25f;
    }
}
