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
    GameObject bossDoor; 

    void Start()
    {
        full = Resources.Load<Sprite>(Constants.FULL_KEY_ICON);
        Keys.Add(GameObject.Find("key 1").GetComponent<Image>());
        Keys.Add(GameObject.Find("key 2").GetComponent<Image>());
        Keys.Add(GameObject.Find("key 3").GetComponent<Image>());

        if(GameStateController.current.CaveComplete)
        {
            foreach(Image key in Keys)
            {
                key.sprite = full; 
            }
            currIndex = numKeys; 
        }
    }

    void Update()
    {
        if (bossDoor == null)
        {
            //Debug.Log("turn off"); 
            gameObject.SetActive(false);
        }

    }

    public void SetBossDoor(GameObject door)
    {
        bossDoor = door; 
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
