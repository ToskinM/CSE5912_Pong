using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseDoor : MonoBehaviour
{
    public string targetScene;
    private SceneController sceneController;
    public bool toInteriorScene = true;
    private GameObject player;
    private GameObject spawn;
    public string thisScene;
    public AudioSource BGM;

    // Start is called before the first frame update
    void Start()
    {
        BGM = GameObject.Find("Scene BGM").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.Equals(PlayerController.instance.AnaiObject.GetComponent<Collider>()) && PlayerController.instance.AnaiIsActive()) || (other.Equals(PlayerController.instance.MimbiObject.GetComponent<Collider>()) && !PlayerController.instance.AnaiIsActive()))
        {
            //Debug.Log("try to save time " + GameStateController.current.SaveTime() + "")

            //player = other.gameObject;
            spawn = GameObject.Find("Spawner");
            spawn.GetComponent<SpawnPoint>().thisScene = targetScene;
            spawn.GetComponent<SpawnPoint>().previousScene = thisScene;

            toInteriorScene = targetScene.Contains("House");
 //           Debug.Log("going inside " + toInteriorScene);
            
            //SceneManager.LoadScene(targetScene);
            if (toInteriorScene)
            {
                PlayerController.instance.DisableSwitching(); 
//                Debug.Log("go inside"); 
                GameStateController.current.SaveTime();

                while (BGM.volume > 0.01)
                {
                    BGM.volume -= BGM.volume * Time.deltaTime * 0.01f;

                }
                SceneController.current.FadeAndLoadSceneNoLS(targetScene);
                SceneManager.sceneLoaded += DisableCompanionObject;
                //DisableCompanionObject();
            }
            else
            {
                if(targetScene.Equals(Constants.SCENE_VILLAGE))
                    PlayerController.instance.DisableSwitching();
                else
                    PlayerController.instance.EnableSwitching();
                //            Debug.Log("out we go");
                while (BGM.volume > 0.01)
                {
                    BGM.volume -= BGM.volume * Time.deltaTime * 0.01f;

                }
                SceneController.current.FadeAndLoadScene(targetScene);
                SceneManager.sceneLoaded += EnableCompanionObject; 
                //EnableCompanionObject();
            }

        }
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
