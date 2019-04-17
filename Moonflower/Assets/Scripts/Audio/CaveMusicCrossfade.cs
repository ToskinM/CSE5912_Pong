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


    public bool MainEnabled;
    public bool BassEnabled;
    public bool ActionEnabled;

    private float maxVol;

    private float mainVolOnPause;
    private float bassVolOnPause;
    private float actionVolOnPause;

    // Start is called before the first frame update
    void Start()
    {
        maxVol = PlayerPrefs.GetFloat("volumeMusic");
        caveSoundtracks = GetComponents<AudioSource>();
        main = caveSoundtracks[0];
        bass = caveSoundtracks[1];
        action = caveSoundtracks[2];

        currentTrack = main;
        currentTrack.volume = PlayerPrefs.GetFloat("volumeMusic");

        actionVolOnPause = action.volume;
        bassVolOnPause = bass.volume;
        mainVolOnPause = main.volume;

        PlayerCombatController.EngageInCombat += FadeInActionTheme;
        PlayerCombatController.DisengageFromCombat += FadeOutActionTheme;
        AudioManager.OnBGMVolChange += OnVolumeChange;
        GameStateController.OnPaused += StoreVolumesOnPause;

    }

    public void TriggerCaveRoomTransition()
    {
        if (MainEnabled && !BassEnabled)
        {
            CrossfadeMainToBass();
        }
        else if (BassEnabled && !MainEnabled)
        {
            CrossfadeBassToMain();
        }
    }

    void CrossfadeMainToBass()
    {
        float fadeSpeed = 0.2f;
        StartCoroutine(FadeOut(main, 0, fadeSpeed));
        StartCoroutine(FadeIn(bass, maxVol, fadeSpeed));
        MainEnabled = false;
        BassEnabled = true;
    }

    void CrossfadeBassToMain()
    {
        float fadeSpeed = 0.2f;
        StartCoroutine(FadeOut(bass, 0, fadeSpeed));
        StartCoroutine(FadeIn(main, maxVol, fadeSpeed));
        BassEnabled = false;
        MainEnabled = true;
    }

    void FadeInActionTheme()
    {
        StartCoroutine(FadeIn(action, maxVol, 0.2f));
        ActionEnabled = true;
    }

    void FadeOutActionTheme()
    {
        StartCoroutine(FadeOut(action, 0, 0.2f));
        ActionEnabled = false;
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
        StartCoroutine(FadeIn(songToFadeIn, maxVol, fadeSpeed));

        yield break;
    }

    public void OnVolumeChange(float volume)
    {
        AssignAudioManager();

        main.volume = mainVolOnPause / maxVol * volume;
        bass.volume = bassVolOnPause / maxVol * volume;
        action.volume = actionVolOnPause / maxVol * volume;
    }

    void StoreVolumesOnPause(bool paused)
    {
        if (paused)
        {
            mainVolOnPause = main.volume;
            bassVolOnPause = bass.volume;
            actionVolOnPause = action.volume;
            maxVol = PlayerPrefs.GetFloat("volumeMusic");
        }
    }

    void AssignAudioManager()
    {
        if (currentTrack == null)
        {
            currentTrack = gameObject.AddComponent<AudioSource>();
            currentTrack.playOnAwake = false;
        }
    }

    void OnDestroy()
    {
        PlayerCombatController.EngageInCombat -= FadeInActionTheme;
        PlayerCombatController.DisengageFromCombat -= FadeOutActionTheme;
        AudioManager.OnBGMVolChange -= OnVolumeChange;
        GameStateController.OnPaused -= StoreVolumesOnPause;
    }
}
