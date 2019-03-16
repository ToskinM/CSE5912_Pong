using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public Slider musicSlider;
    public Slider effectsSlider;

    void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("volumeMusic", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("volumeEffects", 0.75f);
    }

    void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { MusicChangeCheck(); });
        effectsSlider.onValueChanged.AddListener(delegate { EffectsChangeCheck(); });
    }

    private void MusicChangeCheck()
    {
        PlayerPrefs.SetFloat("volumeMusic", musicSlider.value);
    }
    private void EffectsChangeCheck()
    {
        PlayerPrefs.SetFloat("volumeEffects", effectsSlider.value);
    }

    void Destroy()
    {
        musicSlider.onValueChanged.RemoveAllListeners();
        effectsSlider.onValueChanged.RemoveAllListeners();
    }
}
