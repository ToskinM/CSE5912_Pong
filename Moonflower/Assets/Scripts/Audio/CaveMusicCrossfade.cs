using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMusicCrossfade : MonoBehaviour
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

        PlayerCombatController.EngageInCombat += FadeInActionTheme;
        PlayerCombatController.DisengageFromCombat += FadeOutActionTheme;
    }

    /*
    public void CrossFadeToMain()
    {
        StartCoroutine(CrossFade(currentTrack, main, 0.2f));
        currentTrack = main;
    }

    public void CrossFadeToBass()
    {
        StartCoroutine(CrossFade(currentTrack, bass, 0.2f));
        currentTrack = bass;
    }

    public void CrossFadeToAction()
    {
        StartCoroutine(CrossFade(currentTrack, action, 0.2f));
        currentTrack = action;
    }*/

    void FadeInActionTheme()
    {
        StartCoroutine(FadeIn(action, 1, 0.2f));
    }

    void FadeOutActionTheme()
    {
        StartCoroutine(FadeOut(action, 0, 0.2f));
    }

    IEnumerator FadeIn(AudioSource songToFadeIn, float maxVolume, float fadeSpeed)
    {
        while (songToFadeIn.volume < maxVolume)
        {
            songToFadeIn.volume += fadeSpeed * Time.deltaTime;
            yield return null;
        }

        yield break;
    }

    IEnumerator FadeOut(AudioSource songToFadeOut, float minVolume, float fadeSpeed)
    {
        while (songToFadeOut.volume > minVolume)
        {
            songToFadeOut.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        yield break;
    }

    IEnumerator CrossFade(AudioSource songToFadeOut, AudioSource songToFadeIn, float fadeSpeed)
    {
        StartCoroutine(FadeOut(songToFadeOut, 0, fadeSpeed));
        StartCoroutine(FadeIn(songToFadeIn, 1, fadeSpeed));

        yield break;
    }
}
