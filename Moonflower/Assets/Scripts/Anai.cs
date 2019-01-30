using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anai : MonoBehaviour
{
    public bool playing = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        { playing = !playing; }
        //if (playing)
            //Debug.Log("I AM USING ANAI");
    }
}
