using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPosition : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Anai");
    }

    // Update is called once per frame
    void Update()
    {
        if(target != GameObject.Find("Anai"))
            target = GameObject.Find("Anai");
        transform.position = target.transform.position + new Vector3(0, 5, 0);
    }
}
