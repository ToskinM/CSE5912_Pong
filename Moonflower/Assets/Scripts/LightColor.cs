using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColor : MonoBehaviour
{
    public int dayCycleSeconds = 120;
    public SkyColors skyColors; 

    int[] times;
    Color[] colors;
    Color[] colorCategories;
    Vector3[] positions;
    Vector3[] positionCategories;
    Vector3[] rotations;
    Vector3[] rotationCategories;
    float[] intensities;
    float[] intensityCategories;
    float transitionRate = 0;
    Color newColor;
    float intensity;
    Vector3 position;
    Vector3 rotation;
    Light light;
    bool stop=false;
    int counter = 0;
    float transitionTime; // Amount of time it takes to fade between colors

    void Start()

    {
        if(skyColors != null)
            dayCycleSeconds = skyColors.dayCycleSeconds; 
        transitionTime = dayCycleSeconds / 24f;
        colorCategories = new Color[] { new Color(.3f, .3f, .5f), new Color(1, .2f, 0), new Color(.8f, .7f, .9f), new Color(1, 1, .8f), new Color(.9f, 1, .5f), new Color(1, 0, .2f), new Color(.3f, .15f, .5f), new Color(.3f, .3f, .5f) };
        positionCategories = new Vector3[] { new Vector3(-30, 49, 104), new Vector3(-24, 24, 104), new Vector3(-30, 49, 104), new Vector3(-30, 134, -3), new Vector3(-30, 134, -140), new Vector3(-30, 29, -202), new Vector3(-30, 134, -3), new Vector3(-30, 49, 104) };
        rotationCategories = new Vector3[] { new Vector3(31, 183, 359), new Vector3(8, 188, 352), new Vector3(31, 183, 359), new Vector3(74, 188, 4), new Vector3(50, 354, 174), new Vector3(16, 358, 176), new Vector3(74, 188, 4), new Vector3(31, 183, 359)};
        intensityCategories = new float[] {.2f, 1f, 1f, 1f, 1f, 1f, .4f, .2f};
        colors = new Color[24];
        positions = new Vector3[24];
        rotations = new Vector3[24];
        intensities = new float[24];

        light = GetComponent<Light>();


        //12-6am - night
        float transitionR = 1f / 6f;
        float transition = 0;
        for (int i = 0; i < 6; i++)
        {
            colors[i] = Color.Lerp(colorCategories[0], colorCategories[1], transition);
            positions[i] = Vector3.Lerp(positionCategories[0], positionCategories[1], transition);
            rotations[i] = Vector3.Lerp(rotationCategories[0], rotationCategories[1], transition);
            intensities[i] = Mathf.Lerp(intensityCategories[0], intensityCategories[1], transition);
            transition += transitionR;
        }

        //orange - sunrise (6am - 7am)
        colors[6] = colorCategories[1];
        positions[6] = positionCategories[1];
        rotations[6] = rotationCategories[1];
        intensities[6] = intensityCategories[1];


        //light purple - morning (7am-12pm)
        transitionR = 1f / 5f;
        transition = 0;
        for (int i = 7; i < 12; i++)
        {
            colors[i] = Color.Lerp(colorCategories[2], colorCategories[3], transition);
            positions[i] = Vector3.Lerp(positionCategories[2], positionCategories[3], transition);
            rotations[i] = Vector3.Lerp(rotationCategories[2], rotationCategories[3], transition);
            intensities[i] = Mathf.Lerp(intensityCategories[2], intensityCategories[3], transition);
            transition += transitionR;
        }

        //grey - afternoon (12pm - 4pm)
        transitionR = 1f / 4f;
        transition = 0;
        for (int i = 12; i < 16; i++)
        {
            colors[i] = Color.Lerp(colorCategories[3], colorCategories[4], transition);
            positions[i] = Vector3.Lerp(positionCategories[3], positionCategories[4], transition);
            rotations[i] = Vector3.Lerp(rotationCategories[3], rotationCategories[4], transition);
            intensities[i] = Mathf.Lerp(intensityCategories[3], intensityCategories[4], transition);
            transition += transitionR;
        }

        //dark grey - late afternoon/evening (4pm - 7pm)
        transitionR = 1f / 3f;
        transition = 0;
        for (int i = 16; i < 19; i++)
        {
            colors[i] = Color.Lerp(colorCategories[4], colorCategories[5], transition);
            positions[i] = Vector3.Lerp(positionCategories[4], positionCategories[5], transition);
            rotations[i] = Vector3.Lerp(rotationCategories[4], rotationCategories[5], transition);
            intensities[i] = Mathf.Lerp(intensityCategories[4], intensityCategories[5], transition);
            transition += transitionR;
        }

        //magenta - sunset (7pm - 8pm)
        transitionR = 1f;
        transition = 0;
        for (int i = 19; i < 20; i++)
        {
            colors[i] = Color.Lerp(colorCategories[5], colorCategories[6], transition);
            positions[i] = Vector3.Lerp(positionCategories[5], positionCategories[6], transition);
            rotations[i] = Vector3.Lerp(rotationCategories[5], rotationCategories[6], transition);
            intensities[i] = Mathf.Lerp(intensityCategories[5], intensityCategories[6], transition);
            transition += transitionR;
        }


        //Dark purple - early night (8pm - 11pm)
        transitionR = 1f / 3f;
        transition = 0;
        for (int i = 20; i < 23; i++)
        {
            colors[i] = Color.Lerp(colorCategories[6], colorCategories[7], transition);
            positions[i] = Vector3.Lerp(positionCategories[6], positionCategories[7], transition);
            rotations[i] = Vector3.Lerp(rotationCategories[6], rotationCategories[7], transition);
            intensities[i] = Mathf.Lerp(intensityCategories[6], intensityCategories[7], transition);
            transition += transitionR;
        }

        //Night - (11pm - 12am)
        colors[23] = colorCategories[7];
        positions[23] = positionCategories[7];
        rotations[23] = rotationCategories[7];
        intensities[23] = intensityCategories[7];

        transform.position = positions[counter];
        transform.rotation = Quaternion.Euler(rotations[counter]);
        light.intensity = intensities[counter];
        light.color = colors[counter];

    }
    
    public void SetTime(int time)
    {
        counter = time;
        transitionRate = 0;
    }

    public void Stay(bool stay)
    {
        stop = stay;
    }

    void Update()
    {
        if (!stop)
        {
            if (transitionRate < 1)
            {
                transform.position = Vector3.Lerp(transform.position, positions[counter], Time.deltaTime * transitionRate);
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, rotations[counter], Time.deltaTime * transitionRate));
                light.intensity = Mathf.Lerp(light.intensity, intensities[counter], Time.deltaTime * transitionRate);
                light.color = Color.Lerp(light.color, colors[counter], Time.deltaTime * transitionRate);

                transitionRate += Time.deltaTime / (transitionTime);
            }
            else
            {
                if (counter + 1 == colors.Length)
                {
                    counter = 0;
                }
                else
                {
                    counter++;
                }
                transitionRate = 0;
            }
        }
    }


}
