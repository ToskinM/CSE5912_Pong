using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStat : MonoBehaviour
{
    public string Name;
    public int Strength;
    public int Attack;
    public int Defense;
    public int Health;
    public bool AnaiObject;
    public bool MimbiObject;



    // Start is called before the first frame update
    void Start()
    {

    }
    public int GetHealth ()
    {
        return Health;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
