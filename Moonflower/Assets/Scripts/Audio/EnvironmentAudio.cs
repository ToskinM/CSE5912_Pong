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

        AssignAudioManager();

        audioSource.volume = PlayerPrefs.GetFloat("volumeEnivronment");
        AudioManager.OnBackgroundVolChange += OnVolumeChange;
    }
    void OnVolumeChange(float volume)
    {
        AssignAudioManager();
        audioSource.volume = volume;
    }
    void AssignAudioManager()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnDestroy()
    {
        AudioManager.OnBGMVolChange -= OnVolumeChange;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
