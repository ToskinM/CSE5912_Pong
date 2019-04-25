using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoCompanionsAllowed : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.GetCompanionObject().SetActive(false);
        PlayerController.instance.canSwitchCharacters = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
