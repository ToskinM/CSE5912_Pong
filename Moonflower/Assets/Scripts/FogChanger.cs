using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool lavaTrigger;
    bool isNormal = true;
    Color normalColor;
    Color lavaColor;
    Color shadowColor;
    void Start()
    {
        normalColor = new Color(.4f, .4f, .8f);
        lavaColor = new Color(.75f, .3f, .03f);
        shadowColor = new Color(.03f, .03f, .03f);
    }

    // Update is called once per frame
    void Update()
    {
        print(isNormal);
    }

    void OnTriggerStay()
    {
        if (isNormal)
        {
            if (lavaTrigger)
            {
                RenderSettings.fogColor = lavaColor;
            }
            else
            {
                RenderSettings.fogColor = shadowColor;
            }
            isNormal = false;
        }
        else
        {

            RenderSettings.fogColor = normalColor;
            isNormal = true;

        }
    }
}
