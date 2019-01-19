using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public bool Paused { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
    }

    public void TogglePause()
    {
        Paused = !Paused;
    }
}
