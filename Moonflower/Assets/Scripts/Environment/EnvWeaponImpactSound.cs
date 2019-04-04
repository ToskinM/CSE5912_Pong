using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvWeaponImpactSound : MonoBehaviour
{
    public AudioClip[] ImpactSoundsList;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHurtbox")
        {
            audioSource.clip = ImpactSoundsList[Random.Range(0, ImpactSoundsList.Length - 1)];
            audioSource.Play();
        }
    }
}
