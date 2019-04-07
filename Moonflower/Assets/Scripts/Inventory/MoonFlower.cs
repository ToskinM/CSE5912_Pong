using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoonFlower : MonoBehaviour
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
        if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE)
            gameObject.GetComponentInParent<MoonFlowerRegenerate>().ReAddObj(gameObject.transform.position, gameObject.transform.rotation);
    }

}
