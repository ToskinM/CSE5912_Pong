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
    CurrentPlayer player;
    public GameObject light;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        normalColor = new Color(.42f, .82f, .79f);
        lavaColor = new Color(.47f, .32f, .56f);
        shadowColor = new Color(.03f, .03f, .03f);
        RenderSettings.ambientIntensity = 0.8f;
        light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider collider)
    {
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
                    RenderSettings.fogColor = shadowColor;
                    RenderSettings.ambientIntensity = 0.3f;
                    light.SetActive(true);
                }
                isNormal = false;
            }
            else
            {

                RenderSettings.fogColor = normalColor;
                isNormal = true;
                RenderSettings.ambientIntensity = 0.8f;
                light.SetActive(false);


            }
        }
    }
}
