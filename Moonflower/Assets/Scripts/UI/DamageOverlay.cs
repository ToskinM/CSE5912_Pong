using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour
{
    Image overlayScreen;

    // Start is called before the first frame update
    void Start()
    {
        overlayScreen = GetComponent<Image>();
        PlayerController.instance.ActivePlayerStats.OnCharacterDamage += FlashOverlay;
    }

    void FlashOverlay(int damage)
    {
        overlayScreen.canvasRenderer.SetAlpha(1);
        Color screenColor = overlayScreen.color;

        float alphaVal;

        if (damage < 10)
        {
            alphaVal = 0.25f;
        }
        else if (damage < 25)
        {
            alphaVal = 0.5f;
        }
        else
        {
            alphaVal = 0.75f;
        }

        screenColor.a = alphaVal;
        overlayScreen.color = screenColor;
        overlayScreen.CrossFadeAlpha(0f, 1f, false);
    }
}
