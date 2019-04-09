using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TejuCrystal : MonoBehaviour
{
    public AudioClip[] TejuCrystalSoundsList;
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
        AudioManager.OnSFXVolChange += OnVolumeChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FireFree()
    {
        audioSource.clip = TejuCrystalSoundsList[Random.Range(0, TejuCrystalSoundsList.Length - 1)];
        audioSource.Play();
        //Debug.Log("I am Playing");
    }

    void OnVolumeChange(float volume)
    {
        audioSource.volume = volume;
    }
    private void OnDestroy()
    {
        AudioManager.OnSFXVolChange -= OnVolumeChange;
    }
}
