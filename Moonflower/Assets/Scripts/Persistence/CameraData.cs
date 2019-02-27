using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraData
{
    FollowCamera cameraController;
    public float xRotation;
    public float yRotation;

    public CameraData(FollowCamera camera)
    {
        cameraController = camera;
        xRotation = cameraController.yRotation;
        yRotation = cameraController.xRotation;
    }

    public void Load()
    {
        cameraController.yRotation = xRotation;
        cameraController.xRotation = yRotation;
    }
}
