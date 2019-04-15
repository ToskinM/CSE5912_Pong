using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHurtboxController : MonoBehaviour, IHurtboxController
{
    public GameObject Source { get; set; }
    public CharacterStats SourceCharacterStats { get; set; }
    public int Damage { get { return damage; } set { damage = value; } }

    private IProjectile projectile;
    public int damage = 1;

    private void Awake()
    {
        projectile = transform.parent.gameObject.GetComponent<IProjectile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //GameObject otherCharacter = LesserNPCController.GetRootmostObjectInLayer(other.transform, "NPC", "Player");
        GameObject otherCharacter = other.transform.root.gameObject;
        if (otherCharacter == null || otherCharacter != Source)
        {
            //Debug.Log(otherCharacter + "&&" + Source);

            //projectile.OnHit(other.transform.root.gameObject);
            projectile.OnHit(otherCharacter);
        }
    }
}
