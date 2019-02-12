using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalFactory
{

    public Sprite GetHealthyPetal()
    {
        return Resources.Load<Sprite>(Constants.HEALTHY_PETAL);
    }

    public Sprite GetDecayPetal1()
    {
        Sprite petal = Resources.Load<Sprite>(Constants.DECAY_PETAL1);
        if (petal == null)
            Debug.Log("Wtf?");
        return petal;
    }

    public Sprite GetDecayPetal2()
    {
        return Resources.Load<Sprite>(Constants.DECAY_PETAL2);
    }

    public Sprite GetDecayPetal3()
    {
        return Resources.Load<Sprite>(Constants.DECAY_PETAL3);
    }
}
