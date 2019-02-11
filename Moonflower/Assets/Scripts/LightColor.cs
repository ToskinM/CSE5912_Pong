using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColor : MonoBehaviour
{

    Color[] colors;
    Vector3[] positions;
    Vector3[] rotations;
    float[] intensities;
    Light light;
    int counter = 0;
    float transitionTime = 5f; // Amount of time it takes to fade between colors

    void Start()

    {

        colors = new Color[7];
        positions = new Vector3[7];
        rotations = new Vector3[7];
        intensities = new float[7];
        light = GetComponent<Light>();

        //orange - sunrise (6am)
        colors[0] = new Color(1, .5f, 0);
        positions[0] = new Vector3(-24, 24, 104);
        rotations[0] = new Vector3(8, 188, 352);
        intensities[0] = 1f;

        //light purple - morning
        colors[1] = new Color(.8f, .7f, .9f);
        positions[1] = new Vector3(-30, 49, 104);
        rotations[1] = new Vector3(31, 183, 359);
        intensities[1] = 1f;

        //light yellow - afternoon
        colors[2] = new Color(1, 1, .8f);
        positions[2] = new Vector3(-30, 134, -3);
        rotations[2] = new Vector3(74, 188, 4);
        intensities[2] = 1f;

        //yellow - late afternoon/evening
        colors[3] = new Color(.9f, 1, .5f);
        positions[3] = new Vector3(-30, 134, -140);
        rotations[3] = new Vector3(50, 354, 174);
        intensities[3] = 1f;

        //magenta - sunset
        colors[4] = new Color(1, 0, .2f);
        positions[4] = new Vector3(-30, 29, -202);
        rotations[4] = new Vector3(16, 358, 176);
        intensities[4] = 1f;

        //Dark purple - early night
        colors[5] = new Color(.5f, 0, .7f);
        positions[5] = new Vector3(-30, 134, -3);
        rotations[5] = new Vector3(74, 188, 4);
        intensities[5] = .6f;

        //Dark blue - late night
        colors[6] = new Color(.3f, .3f, .5f);
        positions[6] = new Vector3(-30, 49, 104);
        rotations[6] = new Vector3(31, 183, 359);
        intensities[6] = 1f;


        transform.position = positions[0];
        transform.rotation = Quaternion.Euler(rotations[0]);
        light.intensity = intensities[0];
        light.color = colors[0];

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
            float intensity = intensities[counter];
            Vector3 position = positions[counter];
            Vector3 rotation = rotations[counter];

            float transitionRate = 0;


            while (transitionRate < 1)
            {
                transform.position = Vector3.Lerp(transform.position, positions[counter], Time.deltaTime * transitionRate);
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, rotations[counter], Time.deltaTime * transitionRate));
                light.intensity = Mathf.Lerp(light.intensity, intensities[counter], Time.deltaTime * transitionRate);
                light.color = Color.Lerp(light.color, colors[counter], Time.deltaTime * transitionRate);

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
