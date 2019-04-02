using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackTransition : MonoBehaviour
{
    AudioSource[] caveSoundtracks;
    AudioSource main;
    AudioSource bass;
    AudioSource action;

    AudioSource currentTrack;

    // Start is called before the first frame update
    void Start()
    {
        caveSoundtracks = GetComponents<AudioSource>();
        main = caveSoundtracks[0];
        bass = caveSoundtracks[1];
        action = caveSoundtracks[2];

        main.volume = 1;
        bass.volume = 0;
        action.volume = 0;

        currentTrack = main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CrossFadeToMain();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            CrossFadeToBass();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            CrossFadeToAction();
        }
    }

    public void CrossFadeToMain()
    {
        StartCoroutine(Fade(currentTrack, main, 0.2f));
        currentTrack = main;
    }

    public void CrossFadeToBass()
    {
        StartCoroutine(Fade(currentTrack, bass, 0.2f));
        currentTrack = bass;
    }

    public void CrossFadeToAction()
    {
        StartCoroutine(Fade(currentTrack, action, 0.2f));
        currentTrack = action;
    }

    IEnumerator Fade(AudioSource currentSound, AudioSource newSound, float fadeSpeed)
    {
        while (currentSound.volume > 0 && newSound.volume < 1)
        {
            currentSound.volume -= fadeSpeed * Time.deltaTime;
            newSound.volume += fadeSpeed * Time.deltaTime;

            yield return null;
        }

        yield break;
    }
}
