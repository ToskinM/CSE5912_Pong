using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TejuCryAudio : MonoBehaviour, IAudio
{
    public AudioClip[] TejuCrySounds;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        SetupAudioSource();

        audioSource.clip = TejuCrySounds[Random.Range(0, TejuCrySounds.Length - 1)];
        audioSource.Play();
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

    public void OnVolumeChange(float volume)
    {
        audioSource.volume = volume;
    }

    private void OnDestroy()
    {
        AudioManager.OnSFXVolChange -= OnVolumeChange;
    }
}
