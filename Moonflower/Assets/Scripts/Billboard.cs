using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera camera;

    void Awake()
    {
        camera = Camera.main;
    }

    void LateUpdate()
    {
        // Always look at the camera
        //transform.rotation = camera.transform.forward;
        //transform.LookAt(camera.transform.position, Vector3.up);
        //transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
