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
        //Destroy();
        //Debug.Log("awake" + enivronmentSlider.value);
        //Debug.Log("i was called");

        enivronmentSlider.value = PlayerPrefs.GetFloat("volumeEnivronment", 0.75f);
        effectsSlider.value = PlayerPrefs.GetFloat("volumeEffects", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("volumeMusic", 0.75f);
        audioManager = AudioManager.instance;

    }

    void Start()
    {
        //Destroy();
        //Debug.Log("start"+enivronmentSlider.value);
        enivronmentSlider.onValueChanged.AddListener(delegate { EnivronmentChangeCheck(); });
        effectsSlider.onValueChanged.AddListener(delegate { EffectsChangeCheck(); });
        musicSlider.onValueChanged.AddListener(delegate { BGMChangeCheck(); });


        //don't understand why sometimes it has null reference for audiomanager...
        if (audioManager==null)
        {
            audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        }
    }

    private void BGMChangeCheck()
    {
        PlayerPrefs.SetFloat("volumeMusic", musicSlider.value);
        audioManager.ChangeBGMVol(musicSlider.value);
    }
    private void EnivronmentChangeCheck()
    {
        PlayerPrefs.SetFloat("volumeEnivronment", enivronmentSlider.value);
        audioManager.ChangeBackgroundVol(enivronmentSlider.value);
    }
    private void EffectsChangeCheck()
    {
        PlayerPrefs.SetFloat("volumeEffects", effectsSlider.value);
        audioManager.ChangeSoundVol(effectsSlider.value);
    }

    void Destroy()
    {
        enivronmentSlider.onValueChanged.RemoveAllListeners();
        effectsSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
    }
}
