using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyForGate : MonoBehaviour
{
    public GameObject target;
    private GateforKey gate;
    public float time = 0;
    public string playerName;
    private GameObject player;

    private AudioSource audioSource;
    private AudioClip activateSound;
    private AudioClip deactivateSound;
    
    // Start is called before the first frame update
    void Start()
    {
        gate = target.GetComponent<GateforKey>();
        player = GameObject.Find(playerName);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        activateSound = Resources.Load("Audio/Misc/Rune5") as AudioClip;
        deactivateSound = Resources.Load("Audio/Misc/Rune3") as AudioClip;


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (player == null) player = GameObject.Find(playerName);

        if (other.Equals(player.GetComponent<BoxCollider>()))
        {
            audioSource.clip = activateSound;
            audioSource.spatialBlend = 1;
            if (time != 0)
            {
                gate.TimedOpenUp(time);
            }
            else
            {
                gate.OpenUp();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.Equals(player.GetComponent<BoxCollider>()))
        {
            audioSource.clip = deactivateSound;
            audioSource.spatialBlend = 1;
            audioSource.Play();
        }
    }
}
