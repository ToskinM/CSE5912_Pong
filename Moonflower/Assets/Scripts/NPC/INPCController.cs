using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCController
{
    Sprite icon { get; set; }
    bool[] actionsAvailable { get; }
    NPCMovementController movement { get; } 

    void Talk();
    void Gift(string giftName);
    void Distract(GameObject distractedBy);
    void EndDistract();
    string Inspect();
}