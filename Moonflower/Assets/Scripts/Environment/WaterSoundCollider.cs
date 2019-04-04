using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSoundCollider : MonoBehaviour
{
    public AudioClip[] SplashSoundsList;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.clip = SplashSoundsList[Random.Range(0, SplashSoundsList.Length - 1)];
            audioSource.Play();
        }
    }
}
