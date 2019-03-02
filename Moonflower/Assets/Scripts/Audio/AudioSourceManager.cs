using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    private GameObject Anai;
    private GameObject Mimbi;
    private List<GameObject> NPCs;
    private GameObject CurrentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayer = LevelManager.current.currentPlayer;
        NPCs = LevelManager.current.npcs;
    }

    public AudioSource AddAnaiAudioSource()
    {
        Anai = LevelManager.current.anai.gameObject;
        AudioSource audioSource = Anai.AddComponent<AudioSource>();
        return audioSource;
    }
    public AudioSource AddMimbiAudioSource()
    {
        Mimbi = LevelManager.current.mimbi.gameObject;
        return Mimbi.AddComponent<AudioSource>();
    }
    public AudioSource AddCurrentPlayerAudioSource()
    {
        CurrentPlayer = LevelManager.current.currentPlayer;
        return CurrentPlayer.AddComponent<AudioSource>();
    }

    public AudioSource AddNPCPlayerAudioSource()
    {
        foreach (GameObject npc in LevelManager.current.npcs)
        {
            if (npc.name.Contains("Mouse"))
                return npc.AddComponent<AudioSource>();
        }
        return NPCs == null ? null : (AudioSource)null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
