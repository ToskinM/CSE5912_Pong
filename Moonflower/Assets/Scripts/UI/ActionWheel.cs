using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionWheel : MonoBehaviour
{
    private Image UIImage;

    void Start()
    {
        UIImage = GetComponent<Image>();
    }

    public void HoverFocus()
    {
        Color temp = UIImage.color;
        temp.a = 1f;
        UIImage.color = temp;
    }

    public void HoverDefocus()
    {
        Color temp = UIImage.color;
        temp.a = 0.5f;
        UIImage.color = temp;
    }
}
