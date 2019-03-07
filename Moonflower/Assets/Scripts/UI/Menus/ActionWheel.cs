using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionWheel : MonoBehaviour
{
    public Image icon;

    public void Initialize(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }

    void Update()
    {
        
    }
}
