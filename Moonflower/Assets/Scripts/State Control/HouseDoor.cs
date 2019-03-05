using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseDoor : MonoBehaviour
{
    public string targetScene;
    private SceneController sceneController;
    public bool toInteriorScene = true;

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
        if (other.Equals(GameObject.Find("Anai").GetComponent<Collider>()) && (GameObject.Find("Anai").GetComponent<AnaiController>().Playing == true) || other.Equals(GameObject.Find("Mimbi").GetComponent<Collider>()) && (GameObject.Find("Mimbi").GetComponent<MimbiController>().Playing == true))
        {
            //SceneManager.LoadScene(targetScene);
            if (toInteriorScene)
            {
                SceneController.current.FadeAndLoadSceneNoLS(targetScene);
            }
            else
            {
                SceneController.current.FadeAndLoadScene(targetScene);
            }
        }
    }
}
