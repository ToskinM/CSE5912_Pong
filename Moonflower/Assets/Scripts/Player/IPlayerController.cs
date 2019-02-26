using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController
{
    GameObject TalkingPartner { get; set; } 
    bool Playing { get; set; }
    //public GameObject Mimbi;
}
