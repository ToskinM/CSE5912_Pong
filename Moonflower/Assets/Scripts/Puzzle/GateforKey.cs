using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateforKey : MonoBehaviour
{
    public AudioClip OpenSound;
    public AudioClip CloseSound;

    private bool opened = false;
    private float openTime = 0;
    private Vector3 targetUpPos;
    private Vector3 originalPos;

    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        targetUpPos = transform.position + new Vector3(0, 4, 0);
        originalPos = transform.position;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(openTime != 0)
        {
            openTime-= Time.deltaTime;
//            print(openTime);
            if(openTime <= 0)
            {
                openTime = 0;
                TimedClose();
                
            }
        }
        if (opened)
        {
            transform.position = Vector3.Lerp(transform.position, targetUpPos, Time.deltaTime * 2);
        }
        if (!opened)
        {
            
            transform.position = Vector3.Lerp(transform.position, originalPos, Time.deltaTime * 2);
        }
    }

    public void OpenUp()
    {
        if (!opened)
        {
            opened = true;
            PlayOpenSound();
        }
    }

    public void Close()
    {
        if (opened)
        {
            opened = false;
            PlayCloseSound();
        }
    }
    public void TimedOpenUp(float time)
    {
        if(!opened)
        {
            openTime = time;
            opened = true;
            PlayOpenSound();
        }
    }

    public void TimedClose()
    {
        print(originalPos);
        opened = false;
        PlayCloseSound();
    }

    void PlayOpenSound()
    {
        audioSource.clip = OpenSound;
        audioSource.Play();
    }

    void PlayCloseSound()
    {
        audioSource.clip = CloseSound;
        audioSource.Play();
    }
}
