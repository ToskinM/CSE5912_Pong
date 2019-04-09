using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDetection : MonoBehaviour
{
    public GameObject SceneBGM;
    CaveMusicCrossfade transitionHandler;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneBGM == null) SceneBGM = GameObject.Find("Scene BGM");
        transitionHandler = SceneBGM.GetComponent<CaveMusicCrossfade>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transitionHandler.TriggerCaveRoomTransition();
        }
    }
}
