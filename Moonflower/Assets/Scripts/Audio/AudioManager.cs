using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            if (s.source ==null)
                s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        FindObjectOfType<AudioManager>().Play("background");
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
    public float GetVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("there is nth");
        }
        return s.source.volume;
    }
}
