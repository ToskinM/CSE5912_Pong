using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAlertOverlay : MonoBehaviour
{
    CharacterStats playerStats;
    Image overlay;
    bool displaying;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerController.instance.ActivePlayerStats;
        overlay = GetComponent<Image>();
        displaying = false;

        Color overlayC = overlay.color;
        overlayC.a = 1.0f;
        overlay.color = overlayC;

        overlay.canvasRenderer.SetAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        float hpFrac = 1.0f * playerStats.CurrentHealth / playerStats.MaxHealth;

        if (hpFrac <= 0.2f && !displaying)
        {
            overlay.CrossFadeAlpha(1f, 1f, true);
            displaying = true;
        }   
        else if (hpFrac > 0.2f && displaying)
        {
            overlay.CrossFadeAlpha(0f, 1f, true);
            displaying = false;
        }
    }
}
