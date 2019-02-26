using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //private AudioList AllSound ;
    public Sound[] sounds;
    public Sound[] backgrounds;

    public float soundVol;
    public float backgroundVol;

    public static AudioManager instance;
    private AudioAvalibleArea avalibleArea;
    private AudioSourceManager audioSources;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        //Assign vol
        soundVol = 1;
        backgroundVol = 1;
        //Get AudioSource
        audioSources = GetComponent<AudioSourceManager>();
        //Assign music clip to audio source
        AssignToAudioSource(sounds, soundVol);
        AssignToAudioSource(backgrounds,backgroundVol);
        //Play Background wind Sound
        PlayBackground("background");
        //Set hearable area
        avalibleArea = GetComponent<AudioAvalibleArea>();


    }

    private void AssignToAudioSource(Sound[] category, float vol)
    {
        foreach (Sound s in category)
        {
            if (s.source == null)
            {
                if (s.name.Contains("Anai"))
                    s.source = audioSources.GetAnaiAudioSource();
                else if (s.name.Contains("Mimbi"))
                    s.source = audioSources.GetMimbiAudioSource();
                //else if (s.name.Contains("Player"))
                    //s.source = audioSources.GetCurrentPlayerAudioSource();
                else
                    s.source = gameObject.AddComponent<AudioSource>();
            }
                
            s.source.clip = s.clip;
            s.source.volume = vol;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }



    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
            Debug.Log("I am Playing "+name+ s.source.clip +" "+ s.clip);
        }
        else if (s.source = null)
        { Debug.Log("why no audio source!?"); }
        else
        { Debug.Log("there is no music source"); }
    }
    public void PlayBackground(string name)
    {
        Sound s = Array.Find(backgrounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
        else if (s.source = null)
        { Debug.Log("why no audio source!?"); }
        else
        { Debug.Log("there is no music source"); }
    }
    public void PlayTest(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.PlayOneShot(s.source.clip);
        }
        else
        { Debug.Log("there is no music source"); }
    }
    public void Pause (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }
    public void ChangeVolume(string name,float vol)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.volume = vol;
    }

    //Change Volume
    public void ChangeSoundVol(float vol)
    {
        soundVol = vol;
    }
    public void ChangeBackgroundVol(float vol)
    {
        backgroundVol = vol;
    }
    public void UpdateVol(Sound[] category, float vol)
    {
        foreach (Sound s in category)
        {
                s.source.volume = vol;
            avalibleArea.HearableArea(s.source, vol);
        }
    }
    //Get Volume
    public float GetBackgroundVolume()
    {
        return backgroundVol;
    }
    public float GetSoundVolume()
    {
        return soundVol;
    }

    void Update()
    {
        UpdateVol(backgrounds,backgroundVol);
        UpdateVol(sounds, soundVol);
    }


}
