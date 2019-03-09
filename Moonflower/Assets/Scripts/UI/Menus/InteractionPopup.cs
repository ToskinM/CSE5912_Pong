using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPopup : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        text.text = "\"E\"" + "  Interact"; // The idea is to change "E" to whatever key is defined as interaction key, i dunno if i can do this
    }

    void Update()
    {
        
    }
}
