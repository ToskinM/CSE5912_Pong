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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period))
        {
            FlashOverlay();
        }
    }

    void FlashOverlay()
    {
        overlayScreen.canvasRenderer.SetAlpha(1);

        Color screenColor = overlayScreen.color;
        screenColor.a = 0.75f;
        overlayScreen.color = screenColor;
        //overlayScreen.CrossFadeAlpha(0.75f, 0.1f, false);
        overlayScreen.CrossFadeAlpha(0f, 1f, false);
    }
}
