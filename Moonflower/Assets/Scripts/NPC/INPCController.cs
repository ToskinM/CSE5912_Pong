using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCController
{
    Sprite icon { get; set; }

    void Talk();
    void Gift(string giftName);
    void Distract();
    void Inspect();
}