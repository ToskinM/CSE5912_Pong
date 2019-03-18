using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    IHurtboxController Hurtbox { get; set; }

    void OnHit(GameObject other);
}
