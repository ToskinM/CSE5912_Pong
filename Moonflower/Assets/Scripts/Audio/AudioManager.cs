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
    public Audio[] audioSounds;
    public Audio[] audioBackgrounds;

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
        StartCoroutine(GetAudioSourceManager());
        //Assign music clip to audio source
        AssignToAudioSource(audioSounds, soundVol);
        AssignToAudioSource(audioBackgrounds, soundVol);
        //Play Background wind Sound
        PlayBackground("Environment", "Wind");
        //Set hearable area
        avalibleArea = GetComponent<AudioAvalibleArea>();


    }

    private IEnumerator GetAudioSourceManager()
    {
        while (audioSources == null)
        {
            audioSources = FindObjectOfType<AudioSourceManager>();
            yield return null;
        }
    }


    public void AssignToAudioSource(Audio[] category, float vol)
    {
        foreach(Audio a in category)
        {
            Sound[] soundList = a.sounds;
            foreach (Sound s in soundList)
            {
                if (s.source == null)
                {
                    ReAddAudioSource(a, s);
                }
                s.source.clip = s.clip;
                s.source.volume = vol;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
    }

    //Play
    public void Play(string category,string name)
    {

        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);
        if (s != null & s.source != null)
        {
            s.source.Play();
            //Debug.Log("I am Playing "+name+ s.source.clip +" "+ s.clip);
        }
        //Add back audio source
        if (s.source == null)
        {
            ReAddAudioSource(a, s);
        }
    }
    public void PlayBackground(string category, string name)
    {

        Audio a = Array.Find(audioBackgrounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);
        if (s != null & s.source != null)
        {
            s.source.Play();
        }
        //Add back audio source
        if (s.source == null)
        {
            ReAddAudioSource(a, s);
        }
    }
   
    //Foot Step
    public void PlaySneakFootStep(string category,string name)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);
        ChangeFootStep(s.source);
        if (s != null & s.source != null)
        {
            s.source.Play();
        }
        //Add back audio source
        if (s.source == null)
        {
            ReAddAudioSource(a, s);
        }
    }


    //Mute
    public void MuteCategoryVol (string category)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound[] soundList = a.sounds;
        foreach (Sound s in soundList)
        {
            s.source.volume = 0;
        }
    }
    //Resume
    public void ResumeCategoryVol (string category)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound[] soundList = a.sounds;
        foreach (Sound s in soundList)
        {
            s.source.volume = soundVol;
        }
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    //Sneaking Change Volume
    public void ChangeFootStep(AudioSource s)
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);
        s.volume = soundVol * 0.3f;
    }
    public void ResumeNormal(string category ,string name)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);
        s.source.volume = soundVol;
    }

    //Change Volume
    public void ChangeSoundVol(float vol)
    {
        soundVol = vol;
        UpdateVol(audioSounds, soundVol);
    }
    public void ChangeBackgroundVol(float vol)
    {
        backgroundVol = vol;
        UpdateVol(backgrounds, backgroundVol);
    }

    //Update Changed Volume
    public void UpdateVol(Audio[] category, float vol)
    {
        foreach (Audio a in category)
        {
            Sound[] soundList = a.sounds;
            foreach (Sound s in soundList)
            {
                s.source.volume = vol;
                avalibleArea.HearableArea(s.source, vol);
            }
        }
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

    //Re-add audio source
    public void ReAddAudioSource(Audio a, Sound s)
    {
        if (a.categoryName.Contains("Anai"))
            s.source = audioSources.AddAnaiAudioSource();
        else if (a.categoryName.Contains("Mimbi"))
            s.source = audioSources.AddMimbiAudioSource();
        //else if (s.name.Contains("Player"))
        //s.source = audioSources.AddCurrentPlayerAudioSource();
        else
            s.source = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {

    }
}
