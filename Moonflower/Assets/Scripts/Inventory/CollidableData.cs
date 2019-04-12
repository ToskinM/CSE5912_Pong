using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableData 
{
    public string name;
    public GameObject obj;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;


    public CollidableData(GameObject GameObj, Vector3 ObjPosition, Quaternion ObjRotation, Vector3 ObjScale)
    {
        obj = GameObj;
        position = ObjPosition;
        rotation = ObjRotation;
        scale = ObjScale;
    }

}
