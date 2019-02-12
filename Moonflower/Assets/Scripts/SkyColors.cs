using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyColors : MonoBehaviour
{

    Color[] colors;
    float[] fogHeights;
    int[] times;
    MeshRenderer thisRend;
    int counter = 0;
    float transitionTime = 5f; // Amount of time it takes to fade between colors


    void Start()

    {
        times = new int[7]; //how many updates to spend on each color
        thisRend = GetComponent<MeshRenderer>();

        colors = new Color[7];
        fogHeights = new float[7];


        //orange - sunrise (6am)
        colors[0] = new Color(.92f, .16f, .2f);
        fogHeights[0] = 1f;
        times[0] = 1; //spend 1 hour on sunrise

        //light purple - morning
        colors[1] = new Color(.9f, .85f, 1);
        fogHeights[1] = .7f;
        times[1] = 4; //spend 4 hours on morning

        //grey - afternoon
        colors[2] = new Color(.75f, .75f, .75f);
        fogHeights[2] = .1f;
        times[2] = 5; //spend 5 hours on early-mid afternoon

        //dark grey - late afternoon/evening
        colors[3] = new Color(.5f, .5f, .5f);
        fogHeights[3] = .4f;
        times[3] = 3; //spend 3 hours on late afternoon

        //magenta - sunset
        colors[4] = new Color(.92f, .16f, .2f);
        fogHeights[4] = .9f;
        times[4] = 1; //spend 1 hour on sunset

        //Dark purple - early night
        colors[5] = new Color(.7f, .08f, .4f);
        fogHeights[5] = .1f;
        times[5] = 3; //spend 3 hours on early night

        colors[6] = new Color(.2f, .11f, .27f);
        fogHeights[6] = .1f;
        times[6] = 6; //spend 6 hours on night;

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

                transitionRate += Time.deltaTime / (transitionTime * times[counter]);
                yield return null;
            }
            if (counter + 1 == colors.Length)
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
