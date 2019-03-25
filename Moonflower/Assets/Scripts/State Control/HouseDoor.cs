﻿using System.Collections;
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.Equals(LevelManager.current.anai.GetComponent<Collider>()) && LevelManager.current.player.AnaiIsActive()) || (other.Equals(LevelManager.current.mimbi.GetComponent<Collider>()) && !LevelManager.current.player.AnaiIsActive()))
        {
            player = other.gameObject;
            spawn = GameObject.Find("Spawner");
            spawn.GetComponent<SpawnPoint>().thisScene = targetScene;
            if (thisScene.Equals("The Village")) {
                spawn.transform.position = GameObject.Find(targetScene + " Spawn").transform.position;
            }
            
            //SceneManager.LoadScene(targetScene);
            if (toInteriorScene)
            {
                while (BGM.volume > 0.01)
                {
                    BGM.volume -= BGM.volume * Time.deltaTime * 0.01f;

                }
                SceneController.current.FadeAndLoadSceneNoLS(targetScene);
                PlayerController.instance.MimbiObject.SetActive(false);
            }
            else
            {
                while (BGM.volume > 0.01)
                {
                    BGM.volume -= BGM.volume * Time.deltaTime * 0.01f;

                }
                SceneController.current.FadeAndLoadScene(targetScene);
                //PlayerController.instance.MimbiObject.SetActive(true);
            }

        }
    }
}
