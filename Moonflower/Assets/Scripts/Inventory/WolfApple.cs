using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfApple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        gameObject.GetComponentInParent<MoonFlowerRegenerate>().ReAddWolfApple(gameObject.transform.position, gameObject.transform.rotation);
    }
}
