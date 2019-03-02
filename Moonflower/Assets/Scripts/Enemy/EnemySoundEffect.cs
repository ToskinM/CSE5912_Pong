using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundEffect : MonoBehaviour
{
    private AudioManager audioManager;
    private readonly string EnemyMouse = "Enemy_Mouse";

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
    public void EnemyWalkingSFX()
    {
        audioManager.Play(EnemyMouse, "Walking");
    }
    public void EnemyAttackSFX()
    {
        audioManager.Play(EnemyMouse, "Attack");
    }
    public void EnemyGetHit()
    {
        audioManager.Play(EnemyMouse, "GetHit");
    }


    // Update is called once per frame
    void Update()
    {

    }
}
