using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour, IHurtboxController
{
    public GameObject Source { get; set; }
    public CharacterStats SourceCharacterStats { get; set; }
    public int Damage { get; set; } = 1;

    private void Awake()
    {
        Source = gameObject.transform.root.gameObject;
        SourceCharacterStats = Source.GetComponent<CharacterStats>();
    }

    void Start()
    {
        Disable();
    }

    public void Enable(int damage)
    {
        gameObject.SetActive(true);
        this.Damage = damage;
    }
    public void Disable()
    {
        gameObject.SetActive(false);
        this.Damage = 0;
    }
}
