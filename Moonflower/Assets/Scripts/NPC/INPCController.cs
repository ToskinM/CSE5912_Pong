using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCController
{
    Sprite Icon { get; set; }

    void Talk();
    void Gift(string giftName);
    void Distract();
    void Inspect();
}