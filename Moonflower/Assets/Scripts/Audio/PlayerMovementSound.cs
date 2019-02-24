using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    public Sound[] sounds;
    public float soundVol;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        soundVol = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
