using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCController
{
    Sprite icon { get; set; }
    bool[] actionsAvailable { get; }

    void Talk();
    void Gift(string giftName);
    void Distract();
    void EndDistract();
    string Inspect();
}