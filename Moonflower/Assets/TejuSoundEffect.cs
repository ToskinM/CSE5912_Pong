using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TejuSoundEffect : MonoBehaviour, IAudio
{
    public AudioClip[] TejuSFX;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        AudioManager.OnBackgroundVolChange += OnVolumeChange;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Earthquake()
    {
        Debug.Log("I am in earthquake");
        audioSource.clip = TejuSFX[0];
        audioSource.Play();
    }

    public void OnVolumeChange(float volume)
    {
        audioSource.volume = volume;
    }
    private void OnDestroy()
    {
        AudioManager.OnBackgroundVolChange -= OnVolumeChange;
    }

}
