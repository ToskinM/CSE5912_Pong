using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateforKey : MonoBehaviour
{
    private bool opened = false;
    public GameObject target;
    private int openTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(openTime != 0)
        {
            openTime--;
            if(openTime == 0)
            {
                TimedClose();
            }
        }
    }

    public void OpenUp()
    {
        if (!opened)
        {
            transform.position += new Vector3(0, 4, 0);
            opened = true;
        }
    }


    public void TimedOpenUp(int time)
    {
        openTime = time;
        if(!opened)
        {
            transform.position += new Vector3(0, 4, 0);
            opened = true;
        }
    }

    public void TimedClose()
    {
        transform.position += new Vector3(0, -4, 0);
        opened = false;
    }
}
