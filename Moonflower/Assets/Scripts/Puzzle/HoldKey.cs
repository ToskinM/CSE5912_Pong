using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldKey : MonoBehaviour
{

    public GameObject target;
    private GateforKey gate;
    public float time = 0;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gate = target.GetComponent<GateforKey>();

    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = LevelManager.current.currentPlayer;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<BoxCollider>()))
        {
            gate.OpenUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gate.Close();
    }
}