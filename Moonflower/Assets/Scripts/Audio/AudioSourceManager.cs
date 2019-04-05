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
        //NPCs = LevelManager.current.npcs;
        //CurrentPlayer = PlayerController.instance.GetActivePlayerObject();
    }

    public AudioSource AddAnaiAudioSource()
    {
        Anai = PlayerController.instance.AnaiObject;
        AudioSource audioSource = Anai.AddComponent<AudioSource>();
        return audioSource;
    }
    public AudioSource AddMimbiAudioSource()
    {
        Mimbi = PlayerController.instance.MimbiObject;
        return Mimbi.AddComponent<AudioSource>();
    }
    public AudioSource AddCurrentPlayerAudioSource()
    {
        CurrentPlayer = PlayerController.instance.GetActivePlayerObject();
        return CurrentPlayer.AddComponent<AudioSource>();
    }

    //public AudioSource AddNPCPlayerAudioSource()
    //{
    //    //foreach (GameObject npc in LevelManager.current.npcs)
    //    //{
    //    //    if (npc.name.Contains("Mouse"))
    //    //        return npc.AddComponent<AudioSource>();
    //    //}
    //    //return NPCs == null ? null : (AudioSource)null;
    //}


    // Update is called once per frame
    void Update()
    {
        
    }
}
