﻿using System.Collections;
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
    CurrentPlayer player;
    private GameObject light;
    private bool lightsOn = false;
    void Start()
    {
        normalColor = new Color(.42f, .82f, .79f);
        lavaColor = new Color(.47f, .32f, .56f);
        shadowColor = new Color(.03f, .03f, .03f);
        RenderSettings.ambientIntensity = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider collider)
    {
        player = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        if (collider.gameObject.name == player.CurrentPlayerObj.name)
        {
            if (isNormal)
            {
                if (lavaTrigger)
                {
                    RenderSettings.fogColor = lavaColor;
                }
                else
                {
                    if (collider.gameObject.name.Equals("Anai"))
                    {
                        print("hi");
                        light = collider.gameObject.transform.GetChild(5).gameObject;
                        light.SetActive(true);
                        lightsOn = true;
                    }
                    RenderSettings.fogColor = shadowColor;
                    RenderSettings.ambientIntensity = 0.3f;
                }
                isNormal = false;
            }
            else
            {
                if(lightsOn)
                {
                    lightsOn = false;
                    light.SetActive(false);
                }
                RenderSettings.fogColor = normalColor;
                isNormal = true;
                RenderSettings.ambientIntensity = 0.8f;


            }
        }
    }
}
