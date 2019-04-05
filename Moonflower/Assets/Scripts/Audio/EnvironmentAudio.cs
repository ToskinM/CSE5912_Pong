using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentAudio : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        audioSource.volume = 0.75f;
        AudioManager.OnBackgroundVolChange += OnVolumeChange;
    }
    void OnVolumeChange(float volume)
    {
        audioSource.volume = volume;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
