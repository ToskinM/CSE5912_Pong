﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    [HideInInspector] public GameObject source;

    public int damage;
    public CharacterStats sourceCharacterStats;

    void Start()
    {
        source = gameObject.transform.root.gameObject;
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
