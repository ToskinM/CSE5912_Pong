using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtboxController
{
    GameObject Source { get; set; }
    CharacterStats SourceCharacterStats { get; set; }
    int Damage { get; set; }
}
