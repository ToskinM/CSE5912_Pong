using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyForGate : MonoBehaviour
{
    public GameObject target;
    private GateforKey gate;
    public float time = 0;
    public string playerName;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        gate = target.GetComponent<GateforKey>();
        player = GameObject.Find(playerName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<BoxCollider>()))
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
