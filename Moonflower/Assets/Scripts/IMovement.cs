using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Actions
{
    Walking, Running, Chilling, Sneaking, Distracting
}

public interface IMovement
{
    Actions Action { get; set; }
    bool Jumping { get; set; }
}
