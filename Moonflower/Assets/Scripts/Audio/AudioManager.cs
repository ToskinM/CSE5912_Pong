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

    // Start is called before the first frame update
    void Awake()
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
        //Assign music clip to audio source
        AssignToAudioSource(sounds, soundVol);
        AssignToAudioSource(backgrounds,backgroundVol);
        //Play Background wind Sound
        FindObjectOfType<AudioManager>().PlayBackground("background");
    }

    private void AssignToAudioSource(Sound[] category, float vol)
    {
        foreach (Sound s in category)
        {
            if (s.source == null)
                s.source = gameObject.AddComponent<AudioSource>();
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
            //Debug.Log("i am playing");
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
            //Debug.Log("i am playing");
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
        }
    }

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
