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

    public CameraData(Camera mainCamera)
    {
        cameraController = mainCamera.GetComponent<FollowCamera>();
        xRotation = cameraController.xRotation;
        yRotation = cameraController.yRotation;
    }

    public void Load()
    {
        cameraController.xRotation = xRotation;
        cameraController.yRotation = yRotation;
    }
}
