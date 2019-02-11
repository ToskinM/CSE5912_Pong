using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseDoor : MonoBehaviour
{
    private SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(GameObject.Find("Anai").GetComponent<Collider>()) || other.Equals(GameObject.Find("Mimbi").GetComponent<Collider>()))
        {
                SceneManager.LoadScene("Anai House");
        }
    }
}
