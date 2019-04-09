using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudio 
{
    void SetupAudioSource();
    void OnVolumeChange(float volume);
}
