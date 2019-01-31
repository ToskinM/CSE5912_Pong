﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimbiMovement : MonoBehaviour
{
    public GameObject targetObject;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public bool playing = false;
    // Start is called before the first frame update
    void Start()
    {
        //targetObject = GameObject.FindGameObjectWithTag("Pyayer");
        //if (targetObject==null)
        //{
        //    Debug.Log("can't find anai");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        { playing = !playing; }
        if (playing)
        {
            //Debug.Log("I AM USING MIMBI");
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetObject.transform.position.x + 1, targetObject.transform.position.y - 0.5f, targetObject.transform.position.z - 1), ref velocity, smoothTime);
        }
}
}
