using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public GameObject Anai;
    public GameObject Mimbi;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public AudioSource GetAnaiAudioSource()
    {
        return Anai.GetComponent<AudioSource>();
    }
    public AudioSource GetMimbiAudioSource()
    {
        return Mimbi.GetComponent<AudioSource>();
    }
    public AudioSource GetCurrentPlayerAudioSource()
    {
        if (Anai.GetComponent<AnaiController>().Playing == true)
            Player = Anai;
        else
            Player = Mimbi;
        return Player.GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
