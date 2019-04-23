using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System; 


public class KeyDisplayController : MonoBehaviour
{
    List<Image> Keys = new List<Image>(); 
    int currIndex = 0;
    const int numKeys = 3; 
    Sprite full; 


    void Start()
    {
        full = Resources.Load<Sprite>(Constants.FULL_KEY_ICON);
        Keys.Add(GameObject.Find("key 1").GetComponent<Image>());
        Keys.Add(GameObject.Find("key 2").GetComponent<Image>());
        Keys.Add(GameObject.Find("key 3").GetComponent<Image>());
    }

    void Update()
    {


    }

    public void GetKey()
    {
        if(currIndex<numKeys)
        {
            Keys[currIndex].sprite = full; 
            currIndex++; 
        }
    }

}
