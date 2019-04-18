using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPopup : MonoBehaviour
{
    public static InteractionPopup instance; 
    public TextMeshProUGUI text;
    public float currDist;
    public bool NotAllowed = false; 


    private bool npcUsing = false;
    private bool itemUsing = false; 

    private void Start()
    {
        instance = this; 
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    public void EnableNPC(float dist)
    { 
        if(!NotAllowed && dist < currDist)
        {
            gameObject.SetActive(true);
            npcUsing = true; 
            currDist = dist; 
            text.text = "'E' to Interact";
        }
    }

    public void EnableItem(float dist, string name)
    {
        if (!NotAllowed && dist < currDist)
        {
            gameObject.SetActive(true);
            itemUsing = true; 
            currDist = dist;
            text.text = "'E' to pickup " + name;
        }
    }

    public void DisableNPC()
    {
        npcUsing = false; 
        currDist = float.MaxValue; 
    }

    public void DisableItem()
    {
        itemUsing = false; 
        currDist = float.MaxValue;
    }

    //private void OnEnable()
    //{
    //    text.text = "\"E\"" + "  Interact"; // The idea is to change "E" to whatever key is defined as interaction key, i dunno if i can do this
    //}

    void Update()
    {
        if(NotAllowed)
        {
            if (npcUsing)
                DisableNPC();
            if (itemUsing)
                DisableItem(); 
        }
        if (!npcUsing && !itemUsing)
        {
            gameObject.SetActive(false); 

        }
    }
}
