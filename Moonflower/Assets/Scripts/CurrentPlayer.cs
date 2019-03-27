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
        CurrentPlayerObj = PlayerAnaiObj;
    }
    private void CheckCurrentPlayer()
    {
        PlayerAnaiObj = LevelManager.current.anai;
        PlayerMimbiObj = LevelManager.current.mimbi;

        if (PlayerAnaiObj.GetComponent<AnaiController>().Playing == true)
            CurrentPlayerObj = PlayerAnaiObj;
        else
            CurrentPlayerObj = PlayerMimbiObj;
    }
    //public GameObject GetCurrentPlayer()
    //{
    //    return CurrentPlayerObj;
    //}
    public GameObject GetAnai()
    {
        return PlayerAnaiObj;
    }
    public GameObject GetMimbi()
    {
        return PlayerMimbiObj;
    }

    public bool IsAnai()
    {
        if (CurrentPlayerObj == PlayerAnaiObj)
            return true;
        else
            return false;
    }

    public bool IsMimbi()
    {
        if (CurrentPlayerObj == PlayerMimbiObj)
            return true;
        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckCurrentPlayer();
    }
}
