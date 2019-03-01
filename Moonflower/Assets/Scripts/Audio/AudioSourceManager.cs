using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    private GameObject Anai;
    private GameObject Mimbi;
    private GameObject CurrentPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        //CurrentPlayer = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetCurrentPlayer();
        CurrentPlayer = LevelManager.current.currentPlayer;

        Anai = LevelManager.current.anai.gameObject;
        Mimbi = LevelManager.current.mimbi.gameObject;
    }

    public AudioSource AddAnaiAudioSource()
    {
        AudioSource audioSource = Anai.AddComponent<AudioSource>();
        return audioSource;
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
