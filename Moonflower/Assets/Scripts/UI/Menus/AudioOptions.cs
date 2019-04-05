using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public Slider enivronmentSlider;
    public Slider effectsSlider;
    public Slider musicSlider;

    private AudioManager audioManager;

    void Awake()
    {
        Destroy();

        enivronmentSlider.value = PlayerPrefs.GetFloat("volumeEnivronment", 0.75f);
        effectsSlider.value = PlayerPrefs.GetFloat("volumeEffects", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("volumeMusic", 0.75f);
        audioManager = AudioManager.instance;

    }

    void Start()
    {
        enivronmentSlider.onValueChanged.AddListener(delegate { EnivronmentChangeCheck(); });
        effectsSlider.onValueChanged.AddListener(delegate { EffectsChangeCheck(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicChangeCheck(); });



        //don't understand why sometimes it has null reference for audiomanager...
        if (audioManager==null)
        {
            audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        }
    }

    private void MusicChangeCheck()
    {
        PlayerPrefs.SetFloat("volumeMusic", musicSlider.value);
        ChangeBGMVol(musicSlider.value);
    }
    private void EnivronmentChangeCheck()
    {
        PlayerPrefs.SetFloat("volumeEnivronment", enivronmentSlider.value);
        ChangeBackgroundVol(enivronmentSlider.value);
    }
    private void EffectsChangeCheck()
    {
        PlayerPrefs.SetFloat("volumeEffects", effectsSlider.value);
        ChangeAudioVol(effectsSlider.value);
    }
    public void ChangeBackgroundVol(float vol)
    {
        if (audioManager == null)
            Debug.Log("死ね！");
        audioManager.ChangeBackgroundVol(vol);
    }
    public void ChangeAudioVol(float vol)
    {
        audioManager.ChangeSoundVol(vol);
    }
    public void ChangeBGMVol(float vol)
    {
        audioManager.ChangeBGMVol(vol);
    }


    void Destroy()
    {
        enivronmentSlider.onValueChanged.RemoveAllListeners();
        effectsSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
    }
}
