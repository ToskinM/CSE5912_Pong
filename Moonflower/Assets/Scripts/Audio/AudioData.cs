using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData : MonoBehaviour
{
    public float SFXVol;
    public float BGMVol;
    // Start is called before the first frame update
    void Start()
    {
        SFXVol = 1;
        BGMVol = 1;
    }

    public float GetBGMVolume()
    {
        return BGMVol;
    }
    public float GetSFXVolume()
    {
        return SFXVol;
    }

    public void ChangeSFXVol(float vol)
    {
        SFXVol = vol;
    }
    public void ChangeBGMVol(float vol)
    {
        BGMVol = vol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
