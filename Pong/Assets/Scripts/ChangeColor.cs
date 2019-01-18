using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    int count = 0;
    int countThresh = 50;
    int i = 0; 
    List<Color> colors = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        colors.Add(Color.black);
        colors.Add(Color.blue);
        colors.Add(Color.grey);
        colors.Add(Color.green);
        colors.Add(Color.white);
        colors.Add(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        if(count==countThresh)
        {
            GetComponent<Renderer>().material.SetColor("_Color", colors[i]);
            i++;
            if(i>=colors.Count)
            {
                i = 0; 
            }
            count = 0; 
        }
        count++; 
    }
}
