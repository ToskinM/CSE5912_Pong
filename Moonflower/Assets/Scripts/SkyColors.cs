using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyColors : MonoBehaviour
{

    Color[] colors;
    Color[] colorCategories;
    float[] fogCategories;
    Color[] fogColorCategories;
    Color[] fogColors;
    float[] fogHeights;
    bool stop = false;
    LightColor lightColor;
    public int dayCycleSeconds=120;
    MeshRenderer thisRend;
    float transitionTime; // Amount of time it takes to fade between colors
    int time = 0;
    public float transitionRate = 0;
    bool colorChanged = false;
    Color newColor;
    float newFog;

    public enum SkyCategory { Day, Sunset,Night }
    public SkyCategory dayNight;

    public int Passout = 14; 

    PostProcessControl camera; 

    void Start()

    {


        camera = GameObject.Find("Main Camera").GetComponent<PostProcessControl>();
        lightColor = GameObject.Find("Directional Light").GetComponent<LightColor>();
        thisRend = GetComponent<MeshRenderer>();
        transitionTime = dayCycleSeconds / 24f;
        colors = new Color[24];
        fogHeights = new float[24];
        fogColors = new Color[24];
        
        colorCategories = new Color[] {new Color(.2f, .11f, .27f), new Color(.92f, .16f, .2f), new Color(.9f, .85f, 1), new Color(.75f, .75f, .75f), new Color(.5f, .5f, .5f), new Color(.92f, .16f, .2f), new Color(.7f, .08f, .4f), new Color(.2f, .11f, .27f) };
        fogCategories = new float[] {.1f, 1f, .7f, .1f, .4f, .9f, .1f, .1f };
        fogColorCategories = new Color[] { new Color(.6f, .8f, .8f), new Color(.97f, .8f, .3f), new Color(.05f, .03f, .3f) };
        //12-6am - night
        float transitionR = 1f / 6f;
        float transition = 0;
        for (int i = 0; i < 6; i++)
        {
            colors[i] = Color.Lerp(colorCategories[0], colorCategories[1], transition);
            fogHeights[i] = Mathf.Lerp(fogCategories[0], fogCategories[1], transition);
            fogColors[i] = fogColorCategories[2];
            transition += transitionR;
        }

        //orange - sunrise (6am - 7am)
        colors[6] = new Color(.92f, .16f, .2f);
        fogHeights[6] = 1f;
        fogColors[6] = fogColorCategories[1];


        //light purple - morning (7am-12pm)
        transitionR = 1f / 5f;
        transition = 0;
        for (int i = 7; i < 12; i++)
        {
            colors[i] = Color.Lerp(colorCategories[2], colorCategories[3], transition);
            fogHeights[i] = Mathf.Lerp(fogCategories[2], fogCategories[3], transition);
            fogColors[i] = fogColorCategories[0];
            transition += transitionR;
        }

        //grey - afternoon (12pm - 4pm)
        transitionR = 1f / 4f;
        transition = 0;
        for (int i = 12; i < 16; i++)
        {
            colors[i] = Color.Lerp(colorCategories[3], colorCategories[4], transition);
            fogHeights[i] = Mathf.Lerp(fogCategories[3], fogCategories[4], transition);
            fogColors[i] = fogColorCategories[0];
            transition += transitionR;
        }

        //dark grey - late afternoon/evening (4pm - 7pm)
        transitionR = 1f / 3f;
        transition = 0;
        for (int i = 16; i < 19; i++)
        {
            colors[i] = Color.Lerp(colorCategories[4], colorCategories[5], transition);
            fogHeights[i] = Mathf.Lerp(fogCategories[4], fogCategories[5], transition);
            fogColors[i] = fogColorCategories[0];
            transition += transitionR;
        }

        //magenta - sunset (7pm - 8pm)
        transitionR = 1f;
        transition = 0;
        for (int i = 19; i < 20; i++)
        {
            colors[i] = Color.Lerp(colorCategories[5], colorCategories[6], transition);
            fogHeights[i] = Mathf.Lerp(fogCategories[5], fogCategories[6], transition);
            fogColors[i] = fogColorCategories[1];
            transition += transitionR;
        }


        //Dark purple - early night (8pm - 11pm)
        transitionR = 1f / 3f;
        transition = 0;
        for (int i = 20; i < 23; i++)
        {
            colors[i] = Color.Lerp(colorCategories[6], colorCategories[7], transition);
            fogHeights[i] = Mathf.Lerp(fogCategories[6], fogCategories[7], transition);
            fogColors[i] = fogColorCategories[2];
            transition += transitionR;
        }

        //Night - (11pm - 12am)
        colors[23] = new Color(.2f, .11f, .27f);
        fogHeights[23] = .1f;
        fogColors[23] = fogColorCategories[2];


        thisRend.material.SetColor("_TintColor", colors[time]);
        newColor = colors[time];
        newFog = fogHeights[time];
        setDayorNight();

        GameStateController.current.RestoreTime(this);

    }

    void Update()

    {
        if (!stop)
        {
            if (transitionRate < 1)
            {
                thisRend.material.SetColor("_TintColor", Color.Lerp(thisRend.material.GetColor("_TintColor"), newColor, Time.deltaTime * transitionRate));
                thisRend.material.SetFloat("_FogHeight", Mathf.Lerp(thisRend.material.GetFloat("_FogHeight"), newFog, Time.deltaTime * transitionRate));
                RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, fogColors[time], Time.deltaTime * transitionRate);
                transitionRate += Time.deltaTime / transitionTime;
            }
            else
            {
                transitionRate = 0;
                if (time + 1 == colors.Length)
                {
                    time = 0;
                }
                else
                {
                    time++;
                }
                newColor = colors[time];
                newFog = fogHeights[time];
                setDayorNight();
            }
        //    Debug.Log("Pass " + Passout); 
            if(!GameStateController.current.Passed && time <= Passout && time >= Passout-1)
            {
          //      camera.PassOut(); 
            }
            if(!GameStateController.current.Passed && time > Passout)
            {
//                Debug.Log("passed"); 
                GameStateController.current.Passed = true;
            }

        }

    }


    public void setDayorNight()
    {
        if (time < 19 && time >=6) 
            dayNight = SkyCategory.Day;
        else if (time == 19)
            dayNight = SkyCategory.Sunset;
        else
            dayNight = SkyCategory.Night;
    }

    public SkyCategory GetDayNight()
    {
        return dayNight;
    }

    //Set time (0-24) (also sets lighting)
    public void SetTime(int newTime)
    {
        time = newTime;
        newColor = colors[time];
        newFog = fogHeights[time];
        transitionRate = 0;
        setDayorNight();
        lightColor.SetTime(newTime);
    }
    
    public int GetTime()
    {
        return time;
    }

    //Pause or resume passing of time (also pauses/resumes lighting)
    public void Stay(bool stay)
    {
        stop = stay;
        lightColor.Stay(stay);
    }
}
