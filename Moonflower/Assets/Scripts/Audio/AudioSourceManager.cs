using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    private GameObject Anai;
    private GameObject Mimbi;
    private GameObject CurrentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayer = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetCurrentPlayer();
        Anai = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetAnai();
        Mimbi = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetMimbi();
    }

    public AudioSource AddAnaiAudioSource()
    {
        return Anai.AddComponent<AudioSource>();
    }
    public AudioSource AddMimbiAudioSource()
    {
        return Mimbi.AddComponent<AudioSource>();
    }
    public AudioSource AddCurrentPlayerAudioSource()
    {
        return CurrentPlayer.AddComponent<AudioSource>();
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
