using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passout : MonoBehaviour
{
    public SkyColors sky;
    public GameObject cam;  
    public AudioSource BGM;

    PostProcessControl cameraPost;

    public bool Passed = false;

    void Start()
    {
        if(sky.GetTime() > sky.Passout)
        {
            Passed = true; 
        }

        cameraPost = cam.GetComponent<PostProcessControl>(); 
    }

    void Update()
    {
        //    Debug.Log("Pass " + Passout); 
        if (!GameStateController.current.Passed && sky.GetTime() <= sky.Passout && sky.GetTime() >= sky.Passout - 1)
        {
            cameraPost.PassOut(); 
        }

        Debug.Log("Comment out below if you want to turn off passout");
        if(!GameStateController.current.Passed && sky.GetTime() == sky.Passout)
        {
            PlayerController.instance.PassOut();
            Invoke("spawnInHouse", 5); 
        }

        if (!GameStateController.current.Passed && sky.GetTime() > sky.Passout)
        {
            //                Debug.Log("passed"); 
            GameStateController.current.Passed = true;
        }
    }

    private void spawnInHouse()
    {
        GameObject spawn = GameObject.Find("Spawner");
        spawn.GetComponent<SpawnPoint>().thisScene = Constants.SCENE_ANAIHOUSE;
        spawn.GetComponent<SpawnPoint>().previousScene = Constants.SCENE_VILLAGE;

        GameStateController.current.SaveTime();

        while (BGM.volume > 0.01)
        {
            BGM.volume -= BGM.volume * Time.deltaTime * 0.01f;

        }
        SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_ANAIHOUSE);
        //PlayerController.instance.GetCompanionObject().SetActive(false);
        PlayerController.instance.MimbiObject.SetActive(false);
        PlayerController.instance.AnaiObject.SetActive(true);
        PlayerController.instance.Revive(); 

    }

}
