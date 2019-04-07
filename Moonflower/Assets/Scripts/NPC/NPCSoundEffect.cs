using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundEffect : MonoBehaviour
{
    private AudioManager audioManager;
    private readonly string NPC = "NPC Mouse";
    private readonly string NPC_Bat = "NPC Bat";
    private readonly string NPC_Human = "NPC Human";
    private readonly string NPC_Orby = "NPC Orby";
    private readonly string NPC_Teju = "NPC BigMouse";

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
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
                audioManager.Play(DecideWhichNPC(), "Walking(1)");
            else
                audioManager.Play(DecideWhichNPC(), "Walking(2)");
        }

    }

    public void NPCRuningSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        audioManager.Play(DecideWhichNPC(), "Runing");
    }

    public void NPCAttackSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        audioManager.Play(DecideWhichNPC(), "Attack");
    }
    public void NPCGetHit()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        audioManager.Play(DecideWhichNPC(), "GetHit");
    }

    public void NPCIdleSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        audioManager.Play(DecideWhichNPC(), "Idle");
    }
    public void NPCStopIdleSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        audioManager.Stop(DecideWhichNPC(), "Idle");
    }


    public void NPCSpellSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        int x = Random.Range(1, 3);
        if (x == 1)
            audioManager.Play(DecideWhichNPC(), "Spell(1)");
        else
            audioManager.Play(DecideWhichNPC(), "Spell(2)");
        Debug.Log("playing");
    }

    public void NPCSnoringSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        audioManager.Play(DecideWhichNPC(), "Snoring");
    }

    public void NPCGetKillSFX()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
            audioManager.ReAddAllAudioSource(gameObject, DecideWhichNPC());
        audioManager.Play(DecideWhichNPC(), "GetKill");
    }

    public string DecideWhichNPC()
    {
        if (gameObject.name.Contains("Teju"))
            return NPC_Teju;
        else if (gameObject.name.Contains("Mouse"))
            return NPC;
        else if (gameObject.name.Contains("Bat"))
            return NPC_Bat;
        else if (gameObject.name.Contains("Orby"))
            return NPC_Orby;
        else
            return NPC_Human;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
