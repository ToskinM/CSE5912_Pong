using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSphere : MonoBehaviour
{
    private Vector3 startingPosition;
    private bool inside = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && inside)
        {
            GameObject.Find("Anai").transform.position = startingPosition;
            GameObject.Find("Mimbi").transform.position = startingPosition - new Vector3(3, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.Equals(GameObject.Find("Anai").GetComponent<Collider>()) && (GameObject.Find("Anai").GetComponent<AnaiController>().Playing == true) || other.Equals(GameObject.Find("Mimbi").GetComponent<Collider>()) && (GameObject.Find("Mimbi").GetComponent<MimbiController>().Playing == true)) && !inside)
        {
            player = other.gameObject;
            startingPosition = player.transform.position;
            if (player.Equals(GameObject.Find("Anai")))
            {
               GameObject.Find("Mimbi").transform.position = startingPosition - new Vector3(3, 0, 0);
            }
            else  if (player.Equals(GameObject.Find("Mimbi")))
            {  
               GameObject.Find("Anai").transform.position = startingPosition - new Vector3(3, 0, 0);              
            }
            //GameObject.Find("Anai").GetComponent<AnaiController>().InPuzzle = true;
            //GameObject.Find("Mimbi").GetComponent<MimbiController>().InPuzzle = true;
            inside = true;
        }
        print("enter");
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.Equals(GameObject.Find("Anai").GetComponent<Collider>()) && (GameObject.Find("Anai").GetComponent<AnaiController>().Playing == true) || other.Equals(GameObject.Find("Mimbi").GetComponent<Collider>()) && (GameObject.Find("Mimbi").GetComponent<MimbiController>().Playing == true)) && inside)
        {
           //GameObject.Find("Anai").GetComponent<AnaiController>().InPuzzle = false;
            //GameObject.Find("Mimbi").GetComponent<MimbiController>().InPuzzle = false;
            inside = false;
        }
        print("exit");
    }
}
