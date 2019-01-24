using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenCaptureUI : MonoBehaviour
{
    public GameObject textPrefab;
    private float displayDuration = 2f;

    public void DisplayScreencapText()
    {
        Transform canvas = GameObject.Find("Utility Canvas").transform;
        GameObject text = Instantiate(textPrefab, canvas);

        text.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(0.0f, displayDuration, false); // Fade out text
        Destroy(text, displayDuration);
    }
}
