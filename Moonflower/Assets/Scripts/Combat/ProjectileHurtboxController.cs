using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHurtboxController : MonoBehaviour, IHurtboxController
{
    public GameObject Source { get; set; }
    public CharacterStats SourceCharacterStats { get; set; }
    public int Damage { get; set; } = 1;

    private IProjectile projectile;

    private void Awake()
    {
        projectile = transform.parent.gameObject.GetComponent<IProjectile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        projectile.OnHit(other.transform.root.gameObject);
    }
}
