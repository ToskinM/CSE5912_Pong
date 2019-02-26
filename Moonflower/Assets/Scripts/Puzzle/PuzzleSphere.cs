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
            player.transform.position = startingPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(GameObject.Find("Anai").GetComponent<Collider>()) && (GameObject.Find("Anai").GetComponent<AnaiController>().Playing == true) || other.Equals(GameObject.Find("Mimbi").GetComponent<Collider>()) && (GameObject.Find("Mimbi").GetComponent<MimbiController>().Playing == true))
        {
            player = other.gameObject;
            startingPosition = other.transform.position;
            inside = true;
        }
    }
}
