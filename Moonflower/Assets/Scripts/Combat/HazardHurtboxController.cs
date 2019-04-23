using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardHurtboxController : MonoBehaviour, IHurtboxController
{
    [HideInInspector] public GameObject Source { get { return null; } set { source = value; } }
    [HideInInspector] public int Damage { get { return damage; } set { damage = value; } }
    [HideInInspector] public CharacterStats SourceCharacterStats { get; set; }

    public int damage;
    private GameObject source;

    private void Awake()
    {
        //Source = source;
        //SourceCharacterStats = Source.GetComponent<CharacterStats>();
//        Debug.Log("hi");
    }

    void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerController.instance.AnaiIsActive())
            PlayerController.instance.GetActivePlayerObject().GetComponent<PlayerSoundEffect>().AnaiIntoLava();
    }
}
