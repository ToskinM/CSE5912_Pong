using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool lavaTrigger;
    bool isNormal = true;
    bool anaiLavaTriggered = false;
    bool mimbiLavaTriggered = false;
    bool anaiMazeTriggered = false;
    bool mimbiMazeTriggered = false;
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
        player = GameObject.Find("Player").GetComponent<PlayerMovementController>();
        string name = collider.gameObject.name;
        if (name == "Anai")
            {
                if (!anaiLavaTriggered && !anaiMazeTriggered)
                {
                    anaiLavaTriggered = lavaTrigger;

                    if (!lavaTrigger)
                    {
                        light = collider.gameObject.transform.GetChild(5).gameObject;
                        light.SetActive(true);
                        lightsOn = true;
                        anaiMazeTriggered = true;
                    }
                } else
                {
                    if (lightsOn)
                    {
                        lightsOn = false;
                        light.SetActive(false);
                        anaiMazeTriggered = false;
                        anaiLavaTriggered = false;
                    }
                }
         }
         else if (name=="Mimbi") {

            if (!mimbiLavaTriggered && !mimbiMazeTriggered)
            {
                mimbiLavaTriggered = lavaTrigger;
                mimbiMazeTriggered = !lavaTrigger;
            }
            else
            {
                mimbiMazeTriggered = false;
                mimbiLavaTriggered = false;
            }

        }
        if (name == player.playerObject.name)
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
                    RenderSettings.ambientIntensity = 0.3f;
                }
                isNormal = false;
            }
            else
            {
                RenderSettings.fogColor = normalColor;
                isNormal = true;
                RenderSettings.ambientIntensity = 0.8f;
            }
        }
    }

    void CheckLights()
    {
        if (player.playerObject.name == "Anai")
        {
            if (isNormal && anaiLavaTriggered || anaiMazeTriggered)
            {
                if (anaiLavaTriggered)
                {
                    RenderSettings.fogColor = lavaColor;
                    RenderSettings.ambientIntensity = 0.8f;
                } else
                {
                    RenderSettings.fogColor = shadowColor;
                    RenderSettings.ambientIntensity = 0.3f;
                }
                isNormal = false;
            } else if (!isNormal && (!anaiLavaTriggered && !anaiMazeTriggered))
            {
                RenderSettings.fogColor = normalColor;
                isNormal = true;
                RenderSettings.ambientIntensity = 0.8f;
            }
        }
        else
        {
            if (isNormal && mimbiLavaTriggered || mimbiMazeTriggered)
            {
                if (mimbiLavaTriggered)
                {
                    RenderSettings.fogColor = lavaColor;
                    RenderSettings.ambientIntensity = 0.8f;
                }
                else
                {
                    RenderSettings.fogColor = shadowColor;
                    RenderSettings.ambientIntensity = 0.3f;
                }
                isNormal = false;
            }
            else if (!isNormal && (!mimbiLavaTriggered && !mimbiMazeTriggered))
            {
                RenderSettings.fogColor = normalColor;
                isNormal = true;
                RenderSettings.ambientIntensity = 0.8f;
            }
        }
    }
}
