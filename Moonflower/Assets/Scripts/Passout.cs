using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Passout : MonoBehaviour
{
    public SkyColors sky;
    public GameObject cam;  
    public AudioSource BGM;
    //public GameObject cave;  

    private FeedbackText feedback; 

    PostProcessControl cameraPost;

    HouseDoor houseSpawn; 

    public bool Passed = false;

    void Start()
    {
        Passed = GameStateController.current.Passed;

        if (sky.GetTime() > sky.Passout)
        {
            Passed = true;
            GameStateController.current.Passed = true;
        }
        //if (!Passed)
        //{
        //    cave.SetActive(false); 
        //  //  Debug.Log("no cave"); 
        //}

        cameraPost = cam.GetComponent<PostProcessControl>();
        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();


        
    }

    void Update()
    {
        if(!Passed && Input.GetKeyDown(KeyCode.P))
        {
            sky.SetTime(sky.Passout-1);
        }

        //    Debug.Log("Pass " + Passout); 
        if (!GameStateController.current.Passed && sky.GetTime() <= sky.Passout && sky.GetTime() >= sky.Passout - 1)
        {
            cameraPost.PassOut();
            string hot = "It's getting hot...";
            if (!feedback.IsRepeat(hot))
                 feedback.ShowText(hot); 
        }

        Debug.Log("Comment out below if you want to turn off passout");
        if(!GameStateController.current.Passed && sky.GetTime() == sky.Passout)
        {
            PlayerController.instance.PassOut();
            string tooHot = "You got heat-stroke.";
            if (!feedback.IsRepeat(tooHot))
                feedback.ShowText(tooHot);
            Invoke("spawnInHouse", 5);
            GameStateController.current.SaveTime();
            GameStateController.current.Passed = true;
        }

        if (!GameStateController.current.Passed && sky.GetTime() > sky.Passout)
        {
            //                Debug.Log("passed"); 
            GameStateController.current.Passed = true;
        }

//        if(Passed)
//        {
////            Debug.Log("cave good to go!");
        //    cave.SetActive(true);
        //}
    }

    private void spawnInHouse()
    {
        //houseSpawn
        GameObject spawn = GameObject.Find("Spawner");
        spawn.GetComponent<SpawnPoint>().thisScene = Constants.SCENE_ANAIHOUSE;
        spawn.GetComponent<SpawnPoint>().previousScene = Constants.SCENE_VILLAGE;
        PlayerController.instance.DisableSwitching();

        GameStateController.current.SaveTime();

        while (BGM.volume > 0.01)
        {
            BGM.volume -= BGM.volume * Time.deltaTime * 0.01f;

        }
        SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_ANAIHOUSE);
        //PlayerController.instance.GetCompanionObject().SetActive(false);
        SceneManager.sceneLoaded += DisableCompanionObject;
        //PlayerController.instance.MimbiObject.SetActive(false);
        SceneManager.sceneLoaded += ResetPlayer;


    }

    private void ResetPlayer(Scene scene, LoadSceneMode mode)
    {
        PlayerController.instance.AnaiObject.SetActive(true);
        PlayerController.instance.Revive();
    }


    private void EnableCompanionObject(Scene scene, LoadSceneMode mode)
    {
        PlayerController.instance.GetCompanionObject().SetActive(true);
    }

    private void DisableCompanionObject(Scene scene, LoadSceneMode mode)
    {
        PlayerController.instance.GetCompanionObject().SetActive(false);
    }

}
