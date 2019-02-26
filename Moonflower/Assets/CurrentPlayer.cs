using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayer : MonoBehaviour
{
    public GameObject PlayerAnaiObj;
    public GameObject PlayerMimbiObj;
    public GameObject CurrentPlayerObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void CheckCurrentPlayer()
    {
        PlayerAnaiObj = GameObject.Find("Anai");
        PlayerMimbiObj = GameObject.Find("Mimbi");

        if (PlayerAnaiObj.GetComponent<AnaiController>().Playing == true)
            CurrentPlayerObj = PlayerAnaiObj;
        else
            CurrentPlayerObj = PlayerMimbiObj;
    }
    public GameObject GetCurrentPlayer()
    {
        return CurrentPlayerObj;
    }
    // Update is called once per frame
    void Update()
    {
        CheckCurrentPlayer();
    }
}
