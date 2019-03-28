using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessControl : MonoBehaviour
{
    // Start is called before the first frame update
    public PostProcessingProfile passOutProfile;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PassOut()
    {
//        Debug.Log("do it"); 
        GetComponent<PostProcessingBehaviour>().profile = passOutProfile;
    }
}
