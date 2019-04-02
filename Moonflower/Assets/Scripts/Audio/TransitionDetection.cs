using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDetection : MonoBehaviour
{
    public enum CaveSoundtracks { Main, Bass, Action };
    public CaveSoundtracks PostTransitionTrack;

    public GameObject SceneBGM;
    SoundtrackTransition transitionHandler;

    // Start is called before the first frame update
    void Start()
    {
        transitionHandler = SceneBGM.GetComponent<SoundtrackTransition>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (PostTransitionTrack)
            {
                case CaveSoundtracks.Main:
                    transitionHandler.CrossFadeToMain();
                    break;
                case CaveSoundtracks.Bass:
                    transitionHandler.CrossFadeToBass();
                    break;
                case CaveSoundtracks.Action:
                    transitionHandler.CrossFadeToAction();
                    break;
            }
        }
    }
}
