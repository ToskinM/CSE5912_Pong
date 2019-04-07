using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAudio : MonoBehaviour
{
    //public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        for (int index = 0; index < sources.Length; ++index)
        {
            sources[index].mute = true;
        }
    }
}
