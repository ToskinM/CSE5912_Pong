using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ICombatant
{
    public CharacterStats Stats { get; private set; }

    public bool IsBlocking { get; private set; }
    public bool hasWeaponOut;
    public bool inCombat;
    //public bool isBlocking;

    public GameObject weapon;
    public GameObject weaponHitbox;

    private bool isAttacking;

    private bool fireButtonDown;
    private bool sheatheButtonDown;
    private const string ATTACK_AXIS = "Attack";
    private const string SHEATHE_AXIS = "Sheathe/Unsheathe";

    void Start()
    {
        Stats = gameObject.GetComponent<CharacterStats>();

        if (hasWeaponOut)
        {
            weapon.SetActive(true);
        }
        else
        {
            weapon.SetActive(false);
        }
    }

    void Update()
    {
        // Detect attack input (on button down)
        if (Input.GetAxisRaw(ATTACK_AXIS) != 0)
        {
            if (fireButtonDown == false)
            {
                Attack();
                
                fireButtonDown = true;
            }
        }
        if (Input.GetAxisRaw(ATTACK_AXIS) == 0)
        {
            fireButtonDown = false;
        }

        // Detect sheathing input (on button down)
        if (Input.GetAxisRaw(SHEATHE_AXIS) != 0)
        {
            if (sheatheButtonDown == false)
            {
                SetWeaponSheathed(hasWeaponOut);

                sheatheButtonDown = true;
            }
        }
        if (Input.GetAxisRaw(SHEATHE_AXIS) == 0)
        {
            sheatheButtonDown = false;
        }
    }

    // Sheathe/Unsheathe weapon
    private void SetWeaponSheathed(bool sheathed)
    {
        if (sheathed)
            Sheathe();
        else
            Unsheathe();
    }
    private void Sheathe()
    {
        Debug.Log("Sheathing");
        if (hasWeaponOut)
        {
            weapon.SetActive(false);

            hasWeaponOut = false;
        }
    }
    private void Unsheathe()
    {
        Debug.Log("Unsheathing");
        if (!hasWeaponOut)
        {
            weapon.SetActive(true);

            hasWeaponOut = true;
        }
    }

    private void Attack()
    {
        Debug.Log("Attacking");
        if (!hasWeaponOut)   // take out weapon if its not already out
        {
            SetWeaponSheathed(false);
        }
        else
        {
            if (!isAttacking)
            {
                StartCoroutine(Swing());
            }
            // attack
        }
    }

    private IEnumerator Swing()
    {
        isAttacking = true;
        weaponHitbox.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        weaponHitbox.SetActive(false);
        isAttacking = false;
    }
}
