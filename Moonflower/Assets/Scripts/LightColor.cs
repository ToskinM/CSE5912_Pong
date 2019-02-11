using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColor : MonoBehaviour
{
    int[] times;
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
        times = new int[7]; //how many updates to spend on each color
        light = GetComponent<Light>();

        //orange - sunrise (5-6am)
        colors[0] = new Color(1, .2f, 0);
        positions[0] = new Vector3(-24, 24, 104);
        rotations[0] = new Vector3(8, 188, 352);
        intensities[0] = 1f;
        times[0] = 1; //spend 1 hour on sunrise

        //light purple - morning(6-10)
        colors[1] = new Color(.8f, .7f, .9f);
        positions[1] = new Vector3(-30, 49, 104);
        rotations[1] = new Vector3(31, 183, 359);
        intensities[1] = 1f;
        times[1] = 4; //spend 4 hours on morning

        //light yellow - afternoon(11-4)
        colors[2] = new Color(1, 1, .8f);
        positions[2] = new Vector3(-30, 134, -3);
        rotations[2] = new Vector3(74, 188, 4);
        intensities[2] = 1f;
        times[2] = 5; //spend 5 hours on early-mid afternoon
        

        //yellow - late afternoon/evening (4-7)
        colors[3] = new Color(.9f, 1, .5f);
        positions[3] = new Vector3(-30, 134, -140);
        rotations[3] = new Vector3(50, 354, 174);
        intensities[3] = 1f;
        times[3] = 3; //spend 3 hours on late afternoon

        //magenta - sunset (7-8pm)
        colors[4] = new Color(1, 0, .2f);
        positions[4] = new Vector3(-30, 29, -202);
        rotations[4] = new Vector3(16, 358, 176);
        intensities[4] = 1f;
        times[4] = 1; //spend 1 hour on sunset

        //Dark purple - early night (8-11)
        colors[5] = new Color(.3f, .15f, .5f);
        positions[5] = new Vector3(-30, 134, -3);
        rotations[5] = new Vector3(74, 188, 4);
        intensities[5] = .4f;
        times[5] = 3; //spend 3 hours on early night

        //Dark blue - late night (11pm-5am)
        colors[6] = new Color(.3f, .3f, .5f);
        positions[6] = new Vector3(-30, 49, 104);
        rotations[6] = new Vector3(31, 183, 359);
        intensities[6] = .2f;
        times[6] = 6; //spend 6 hours on night;


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
