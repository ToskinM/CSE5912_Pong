using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool lavaTrigger;
    public FogChangerInfo fogInfo;
    Color normalColor;
    Color lavaColor;
    Color shadowColor;
    PlayerMovementController player;
    private GameObject light;
    private bool lightsOn = false;
    void Start()
    {
        normalColor = new Color(.42f, .82f, .79f);
        lavaColor = new Color(.47f, .32f, .56f);
        shadowColor = new Color(.03f, .03f, .03f);
        RenderSettings.ambientIntensity = 0.8f;
        player = GameObject.Find("Player").GetComponent<PlayerMovementController>();
        fogInfo = GameObject.Find("FogChangerInfo").GetComponent<FogChangerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckLights();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        
        string name = collider.gameObject.name;
        if (name == "Anai")
            {
                if (!fogInfo.anaiLavaTriggered && !fogInfo.anaiMazeTriggered)
                {
                    fogInfo.anaiLavaTriggered = lavaTrigger;

                    if (!lavaTrigger)
                    {
                        light = collider.gameObject.transform.GetChild(5).gameObject;
                        light.SetActive(true);
                        lightsOn = true;
                        fogInfo.anaiMazeTriggered = true;
                    }
                } else
                {
                    if (lightsOn)
                    {
                        lightsOn = false;
                        light.SetActive(false);
                        fogInfo.anaiMazeTriggered = false;
                        fogInfo.anaiLavaTriggered = false;
                    }
                }
         }
         else if (name=="Mimbi") {

            if (!fogInfo.mimbiLavaTriggered && !fogInfo.mimbiMazeTriggered)
            {
                fogInfo.mimbiLavaTriggered = lavaTrigger;
                fogInfo.mimbiMazeTriggered = !lavaTrigger;
            }
            else
            {
                fogInfo.mimbiMazeTriggered = false;
                fogInfo.mimbiLavaTriggered = false;
            }

        }
        if (name == player.playerObject.name)
        {
            if (fogInfo.isNormal)
            {
                if (lavaTrigger)
                {
                    RenderSettings.fogColor = lavaColor;
                }
                else
                {
                    RenderSettings.fogColor = shadowColor;
                    RenderSettings.ambientIntensity = 0.3f;
                }
                fogInfo.isNormal = false;
            }
            else
            {
                RenderSettings.fogColor = normalColor;
                fogInfo.isNormal = true;
                RenderSettings.ambientIntensity = 0.8f;
            }
        }
    }

    void CheckLights()
    {
        if (player.playerObject.name == "Anai")
        {
            if (fogInfo.isNormal && fogInfo.anaiLavaTriggered || fogInfo.anaiMazeTriggered)
            {
                if (fogInfo.anaiLavaTriggered)
                {
                    RenderSettings.fogColor = lavaColor;
                    RenderSettings.ambientIntensity = 0.8f;
                } else
                {
                    RenderSettings.fogColor = shadowColor;
                    RenderSettings.ambientIntensity = 0.3f;
                }
                fogInfo.isNormal = false;
            } else if (!fogInfo.isNormal && (!fogInfo.anaiLavaTriggered && !fogInfo.anaiMazeTriggered))
            {
                RenderSettings.fogColor = normalColor;
                fogInfo.isNormal = true;
                RenderSettings.ambientIntensity = 0.8f;
            }
        }
        else
        {
            if (fogInfo.isNormal && fogInfo.mimbiLavaTriggered || fogInfo.mimbiMazeTriggered)
            {
                if (fogInfo.mimbiLavaTriggered)
                {
                    RenderSettings.fogColor = lavaColor;
                    RenderSettings.ambientIntensity = 0.8f;
                }
                else
                {
                    RenderSettings.fogColor = shadowColor;
                    RenderSettings.ambientIntensity = 0.3f;
                }
                fogInfo.isNormal = false;
            }
            else if (!fogInfo.isNormal && (!fogInfo.mimbiLavaTriggered && !fogInfo.mimbiMazeTriggered))
            {
                RenderSettings.fogColor = normalColor;
                fogInfo.isNormal = true;
                RenderSettings.ambientIntensity = 0.8f;
            }
        }
    }
}
