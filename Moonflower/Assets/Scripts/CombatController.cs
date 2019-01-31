using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public bool isWeaponOut;
    public bool inCombat;
    public bool isBlocking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Attack") > 0)
            Attack();
    }

    // Sheathe/Unsheathe weapon
    private void SetWeaponSheathe(bool sheathed)
    {
        if (sheathed)
            Unsheathe();
        else
            Sheathe();
    }
    private void Sheathe()
    {
        if (isWeaponOut)
        {


            isWeaponOut = false;
        }
    }
    private void Unsheathe()
    {
        if (!isWeaponOut)
        {


            isWeaponOut = true;
        }
    }

    private void Attack()
    {
        if (!isWeaponOut)
        {
            SetWeaponSheathe(false);
        }
        else
        {
            // attack
        }
    }
}
