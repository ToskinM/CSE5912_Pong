using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public GameObject Anai;
    public GameObject Mimbi;
    private GameObject CurrentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayer = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetCurrentPlayer();
    }

    public AudioSource GetAnaiAudioSource()
    {
        return Anai.AddComponent<AudioSource>();
    }
    public AudioSource GetMimbiAudioSource()
    {
        return Mimbi.AddComponent<AudioSource>();
    }
    public AudioSource GetCurrentPlayerAudioSource()
    {
        return CurrentPlayer.AddComponent<AudioSource>();
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
