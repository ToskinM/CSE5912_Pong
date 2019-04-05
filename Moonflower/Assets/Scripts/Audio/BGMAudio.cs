using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAudio : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {

    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        AssignAudioManager();

        audioSource.volume = PlayerPrefs.GetFloat("volumeMusic");
        AudioManager.OnBGMVolChange += OnVolumeChange;
    }
    void OnVolumeChange(float volume)
    {
        AssignAudioManager();
        if (audioSource != null)
        audioSource.volume = volume;
    }

    void AssignAudioManager()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            //audioSource.playOnAwake = false;
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
