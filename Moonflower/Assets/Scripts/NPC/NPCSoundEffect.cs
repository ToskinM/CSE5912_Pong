using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundEffect : MonoBehaviour
{
    private AudioManager audioManager;
    private readonly string NPC = "NPC Mouse";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetAudioManager());
    }
    public IEnumerator GetAudioManager()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            yield return null;
        }
    }
    public void NPCWalkingSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, NPC);
        audioManager.Play(NPC, "Walking");
    }
    public void NPCAttackSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, NPC);
        audioManager.Play(NPC, "Attack");
    }
    public void NPCGetHit()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, NPC);
        audioManager.Play(NPC, "GetHit");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
