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
        targetDownPos = transform.position - new Vector3(0, 4, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (fall && transform.position != targetDownPos)
        {
            transform.position = Vector3.Lerp(transform.position, targetDownPos, Time.deltaTime * 5);
        }
        
    }
    void OnTriggerEnter(Collider collider)
    {
        player = GameObject.Find("Player").GetComponent<CurrentPlayer>();
        if (collider.gameObject.name == player.CurrentPlayerObj.name && !fall)
        {
            fall = true;
        }
    }
}
