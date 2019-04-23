using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private int numKeys;
    GameObject bossEntrance;
    public bool Open = false;
    public KeyDisplayController display;

    // Start is called before the first frame update
    void Start()
    {
        display = GameObject.Find("Key Display").GetComponent<KeyDisplayController>();
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
            Open = true; 
        }
    }

    public void KeyOpen()
    {
        numKeys--;
        display.GetKey(); 
    }

    public void Unlock()
    {
        bossEntrance.SetActive(true);
    }
}
