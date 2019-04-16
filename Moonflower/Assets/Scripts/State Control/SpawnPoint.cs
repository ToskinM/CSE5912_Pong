using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint current;

    public string thisScene;
    public string previousScene;

    void Start()
    {
        if (current == null)
        {
            Debug.Log(gameObject.GetInstanceID() + " is new singleton");
            DontDestroyOnLoad(gameObject);
            current = this;
        }
        else if (current != this)
        {
            Debug.Log(current.GetInstanceID() + " is current singleton, deleting " + current.GetInstanceID());

            Destroy(gameObject);
        }
    }

    public void Spawn()
    {
        //if (thisScene == null) thisScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (previousScene != "") transform.position = GameObject.Find(previousScene + " Spawn").transform.position;
        PlayerController.instance.GetActivePlayerObject().transform.position = this.transform.position;
        //PlayerController.instance.GetActivePlayerObject().transform.rotation = this.transform.rotation;
        //PlayerController.instance.GetActivePlayerObject().transform.Rotate()

        PlayerController.instance.GetCompanionObject().GetComponent<NavMeshAgent>().Warp(this.transform.position + new Vector3(0, 0, -5));
    }

    public void ResetScenes()
    {
        thisScene = null;
        previousScene = null;
    }
}
