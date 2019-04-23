using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FallingBridge : MonoBehaviour
{
    private bool fall = false;
    private Vector3 targetDownPos;
    CurrentPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        targetDownPos = transform.position - new Vector3(0, 35, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (fall && transform.position != targetDownPos)
        {
            transform.position = Vector3.Lerp(transform.position, targetDownPos, Time.deltaTime * 0.2f);
        }
        
    }
    void OnTriggerStay(Collider collider)
    {

        player = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        if (collider.gameObject == PlayerController.instance.GetActivePlayerObject() && !fall)
        {
            fall = true;
        }
    }
}
