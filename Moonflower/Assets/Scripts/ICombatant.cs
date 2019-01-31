﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatant
{
    CharacterStats Stats { get; }
    bool IsBlocking { get; }
}
