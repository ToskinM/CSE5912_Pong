using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyForGate : MonoBehaviour
{
    public GameObject target;
    public GateforKey gate;
    public int time = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        gate = target.GetComponent<GateforKey>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(GameObject.Find("Anai").GetComponent<BoxCollider>()) || other.Equals(GameObject.Find("Mimbi").GetComponent<BoxCollider>()))
        {
            if (time != 0)
            {
                gate.TimedOpenUp(time);
            }
            else
            {
                gate.OpenUp();
            }
        }
    }
}
