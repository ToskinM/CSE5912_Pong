using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPosition : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        target = LevelManager.current.anai.gameObject;
    }

    void Update()
    {
        if(target != LevelManager.current.anai.gameObject)
            target = LevelManager.current.anai.gameObject;
        transform.position = target.transform.position + new Vector3(0, 5, 0);
    }
}
