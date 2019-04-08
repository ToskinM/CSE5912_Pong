using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private int numKeys;
    GameObject bossEntrance;
    // Start is called before the first frame update
    void Start()
    {
        numKeys = GameObject.FindGameObjectsWithTag("BossKey").Length;
        bossEntrance = GameObject.Find("BossEntrance");
        bossEntrance.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(numKeys == 0)
        {
            Unlock();
            print("GIMME");
        }
    }

    public void KeyOpen()
    {
        numKeys--;
    }

    public void Unlock()
    {
        bossEntrance.SetActive(true);
    }
}
