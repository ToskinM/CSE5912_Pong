using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyColors : MonoBehaviour
{

    Color[] colors;
    float[] fogHeights;
    MeshRenderer thisRend;
    int counter = 0;
    float transitionTime = 5f; // Amount of time it takes to fade between colors

    void Start()

    {

        thisRend = GetComponent<MeshRenderer>();

        colors = new Color[7];
        fogHeights = new float[7];


        //orange - sunrise (6am)
        colors[0] = new Color(.92f, .16f, .2f);
        fogHeights[0] = 1f;
        //light purple - morning
        colors[1] = new Color(.9f, .6f, 1);
        fogHeights[1] = .7f;
        //grey - afternoon
        colors[2] = new Color(.75f, .75f, .75f);
        fogHeights[2] = .1f;
        //dark grey - late afternoon/evening
        colors[3] = new Color(.5f, .5f, .5f);
        fogHeights[3] = .4f;
        //magenta - sunset
        colors[4] = new Color(.92f, .16f, .2f);
        fogHeights[4] = .9f;
        //Dark purple - early night
        colors[5] = new Color(.7f, .08f, .4f);
        fogHeights[5] = .1f;
        colors[6] = new Color(.2f, .11f, .27f);
        fogHeights[6] = .1f;

        thisRend.material.SetColor("_TintColor", colors[0]);
        StartCoroutine(ColorChange());

    }

    void Update()

    {

    }

    IEnumerator ColorChange()

    {

        //Infinite loop will ensure our coroutine runs all game

        while (true)

        {

            Color newColor = colors[counter];
            float newFog = fogHeights[counter];

            float transitionRate = 0; 
            

            while (transitionRate < 1)
            {
                thisRend.material.SetColor("_TintColor", Color.Lerp(thisRend.material.GetColor("_TintColor"), newColor, Time.deltaTime * transitionRate));
                thisRend.material.SetFloat("_FogHeight", Mathf.Lerp(thisRend.material.GetFloat("_FogHeight"), newFog, Time.deltaTime * transitionRate));

                transitionRate += Time.deltaTime / transitionTime;

                yield return null;
            }
            if (counter + 1 > 6)
            {
                counter = 0;
            }
            else
            {
                counter++;
                print(counter);
            }
            yield return null;
        }
    }
}
