using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaAudio : MonoBehaviour, IAudio
{
    public AudioClip TejuCrySounds;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        SetupAudioSource();
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
