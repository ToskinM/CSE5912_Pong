using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ICombatant
{
    public CharacterStats Stats { get; private set; }

    public bool IsBlocking { get; private set; }
    [HideInInspector] public bool hasWeaponOut;
    [HideInInspector] public bool inCombat;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public int attack = 0;

    //public bool isBlocking;

    public GameObject weapon;
    public GameObject[] attackHurtboxes;


    private const float attackDelay = 0.35f;

    private const string ATTACK_AXIS = "Attack";
    private const string BLOCK_AXIS = "Block";
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
        if (Input.GetButtonDown(ATTACK_AXIS))
            Attack();

        // Detect attack input (on button down)
        if (Input.GetButtonDown(BLOCK_AXIS))
            Block(true);
        else if (Input.GetButtonUp(BLOCK_AXIS))
            Block(false);

        // Detect sheathing input (on button down)
        if (Input.GetButtonDown(SHEATHE_AXIS))
        {
            SetWeaponSheathed(hasWeaponOut);
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

        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
        attack = (attack + 1) % 2;
    }

    private void Block(bool block)
    {
        if (block)
        {
            Debug.Log(gameObject.name + " started blocking.");
            IsBlocking = true;
        }
        else
        {
            Debug.Log(gameObject.name + " stopped blocking.");
            IsBlocking = false;
        }
    }

    public void EnableHurtbox(int index)
    {
        attackHurtboxes[index].SetActive(true);
    }
    public void DisableHurtbox(int index)
    {
        attackHurtboxes[index].SetActive(false);
    }
}
