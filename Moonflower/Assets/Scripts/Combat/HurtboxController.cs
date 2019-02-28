using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    [HideInInspector] public GameObject source;
    [HideInInspector] public CharacterStats sourceCharacterStats;

    public int damage;

    private void Awake()
    {
        source = gameObject.transform.root.gameObject;
        sourceCharacterStats = source.GetComponent<CharacterStats>();
    }

    void Start()
    {
        Disable();
    }

    public void Enable(int damage)
    {
        gameObject.SetActive(true);
        this.damage = damage;
    }
    public void Disable()
    {
        gameObject.SetActive(false);
        this.damage = 0;
    }
}
