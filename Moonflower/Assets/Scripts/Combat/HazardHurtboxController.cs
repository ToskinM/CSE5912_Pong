using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardHurtboxController : MonoBehaviour, IHurtboxController
{
    [HideInInspector] public GameObject Source { get { return gameObject; } set { source = value; } }
    [HideInInspector] public int Damage { get { return damage; } set { damage = value; } }
    [HideInInspector] public CharacterStats SourceCharacterStats { get; set; }

    public int damage;
    private GameObject source;

    private void Awake()
    {
        Source = source;
        SourceCharacterStats = Source.GetComponent<CharacterStats>();
//        Debug.Log("hi");
    }

    void Start()
    {
    }
}
