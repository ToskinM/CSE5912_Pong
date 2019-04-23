using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Audio[] audioSounds;
    public Audio[] audioBackgrounds;

    public float soundVol;
    public float backgroundVol;
    public float bgmVol;

    //public GameObject SceneBGM;

    //private AudioAvalibleArea avalibleArea;
    private AudioSourceManager audioSources;

    public delegate void SFXVolumeChange(float volume);
    public delegate void BackgroundVolumeChange(float volume);
    public delegate void BGMVolumeChange(float volume);

    public static event SFXVolumeChange OnSFXVolChange;
    public static event BackgroundVolumeChange OnBackgroundVolChange;
    public static event BGMVolumeChange OnBGMVolChange;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //Assign vol
        backgroundVol = PlayerPrefs.GetFloat("volumeMusic", 0.75f);
        soundVol = PlayerPrefs.GetFloat("volumeEffects", 0.75f);
        bgmVol = PlayerPrefs.GetFloat("volumeMusic", 0.75f);

        //Get AudioSource
        StartCoroutine(GetAudioSourceManager());

        //SceneBGM = GameObject.Find("Scene BGM");
    }

    private IEnumerator GetAudioSourceManager()
    {
        while (audioSources == null)
        {
            audioSources = FindObjectOfType<AudioSourceManager>();
            yield return null;
        }
    }

    //Play
    public void Play(string category,string name)
    {

        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);

        //Add back audio source
        if (s.source == null)
        {
            AddAllAudioSource(category);
            if (a.categoryName=="BGM")
            {
                BMGAddAllAudioSource(category);
            }
        }
        if (s != null & s.source != null)
        {
            s.source.mute = false;
            if (name == "Snoring")
            {//don't play snoring
                if ((s.source.gameObject.name == "BigMouse") || (s.source.gameObject.name == "Teju"))
                {
                    s.source.Play();
                    //Debug.Log("I am Playing " + name + s.source.clip + s.source.gameObject.name);
                }
                else
                { }
                //Debug.Log("I am not Playing " + name + s.source.clip + s.source.gameObject.name);
            }
            else
            {
                s.source.Play();
                //Debug.Log("I am Playing " + name + s.source.clip + s.source.gameObject.name);
            }
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

    public void Stop(string category, string name)
    {

        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);
        if (s != null & s.source != null)
        {
            s.source.Stop();
            s.source.mute = true;
            //Debug.Log("I am disabling "+name+ s.source.clip);
        }
        //Add back audio source
        if (s.source == null)
        {
            AddAllAudioSource(category);
            if (a.categoryName == "BGM")
            {
                BMGAddAllAudioSource(category);
            }
        }
    }




    //Foot Step
    public void PlayFootStep(string category, string name)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);
        if (s.source == null)
        {
            ReAddAudioSource(a, s);
            AddSourceOtherComponent(s);
        }

        ChangeFootStep(s.source, 0.3f);
        if (s != null & s.source != null)
        {
            s.source.Play();
        }
        //Add back audio source

    }

    public void PlaySneakFootStep(string category,string name)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);
        if (s.source == null)
        {
            ReAddAudioSource(a, s);
            AddSourceOtherComponent(s);
        }

        ChangeFootStep(s.source, 0.1f);
        if (s != null & s.source != null)
        {
            s.source.Play();
        }
        //Add back audio source

    }


    //Mute
    public void MuteCategoryVol (string category)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound[] soundList = a.sounds;
        foreach (Sound s in soundList)
        {
            if (s.source!=null)
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
            if (s.source != null)
                s.source.volume = soundVol;
        }
    }

    //public void Pause(string name)
    //{
    //    Sound s = Array.Find(sounds, sound => sound.name == name);
    //    s.source.Pause();
    //}

    //Sneaking Change Volume
    public void ChangeFootStep(AudioSource s, float vol)
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);
        s.volume = soundVol * vol;
    }
    public void ResumeNormal(string category ,string name)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound s = Array.Find(a.sounds, sound => sound.name == name);
        if (s.source != null)
            s.source.volume = soundVol;
    }

    //Change Volume
    public void ChangeSoundVol(float vol)
    {
        soundVol = vol;
        UpdateVol(audioSounds, soundVol);
        OnSFXVolChange?.Invoke(soundVol);
    }
    public void ChangeBackgroundVol(float vol)
    {
        backgroundVol = vol;
        UpdateVol(audioBackgrounds, backgroundVol);
        OnBackgroundVolChange.Invoke(backgroundVol);
    }

    public void ChangeBGMVol(float vol)
    {
        bgmVol = vol;
        //UpdateBGMVol(SceneBGM, vol);
        //if (OnBGMVolChange!=null)
            OnBGMVolChange.Invoke(bgmVol);
    }

    //Update Changed Volume
    public void UpdateVol(Audio[] category, float vol)
    {
        foreach (Audio a in category)
        {
            Sound[] soundList = a.sounds;
            foreach (Sound s in soundList)
            {
                if (s.source == null)
                    break;
                s.source.volume = vol;
                //avalibleArea.HearableArea(s.source, vol);
            }
        }
    }

    //public void UpdateBGMVol(GameObject BGMObject, float vol)
    //{
    //    AudioSource[] BGMs = BGMObject.GetComponents<AudioSource>();
    //    foreach (AudioSource a in BGMs)
    //    {
    //        a.volume = vol;
    //    }
    //}

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
        else if (a.categoryName.Contains("BigBig"))
            s.source = audioSources.AddBigMouseSource();
        else if (a.categoryName.Contains("Mouse"))
        {
            //do nothing for now
            /*s.source = audioSources.AddNPCPlayerAudioSource();*/
        }
        else if (s.name.Contains("Player"))
        s.source = audioSources.AddCurrentPlayerAudioSource();
        else
        {
            GameObject x = GameObject.Find("Audio");
            //Add audio source on audio object
            s.source = x.AddComponent<AudioSource>();
        }
            
    }
    //Add all SFX for certain category for certain object
    public void ReAddAllAudioSource(GameObject obj, String category)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound[] soundList = a.sounds;
        foreach (Sound s in soundList)
        {
            s.source = obj.AddComponent<AudioSource>();
            AddSourceOtherComponent(s);
        }

    }
    public void AddSourceOtherComponent(Sound s)
    {
        s.source.clip = s.clip;
        s.source.volume = soundVol;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.spatialBlend = 1;
        s.source.maxDistance = 10;
    }
    public void BGMAddSourceOtherComponent(Sound s)
    {
        s.source.clip = s.clip;
        s.source.volume = soundVol;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.spatialBlend = 0;
    }



    public void AddAllAudioSource(String category)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound[] soundList = a.sounds;
        foreach (Sound s in soundList)
        {
            ReAddAudioSource(a, s);
            AddSourceOtherComponent(s);
        }
    }
    public void BMGAddAllAudioSource(String category)
    {
        Audio a = Array.Find(audioSounds, sound => sound.categoryName.Contains(category));
        Sound[] soundList = a.sounds;
        foreach (Sound s in soundList)
        {
            ReAddAudioSource(a, s);
            BGMAddSourceOtherComponent(s);
        }
    }



}
