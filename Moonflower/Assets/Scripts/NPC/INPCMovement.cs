using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPCMovement
{
    bool Active { get; set; }

    void UpdateMovement();

    void ResumeMovement(); 
}