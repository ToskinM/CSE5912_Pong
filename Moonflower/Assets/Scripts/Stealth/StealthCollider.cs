using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthCollider : MonoBehaviour
{
    public StealthDetection stealthDetection;

    void OnTriggerEnter(Collider other)
    {
        //stealthDetection.TriggerEnterReaction(other);
    }

    void OnTriggerStay(Collider other)
    {
        stealthDetection.TriggerStayReaction(other);
    }

    void OnTriggerExit(Collider other)
    {
        //stealthDetection.TriggerExitReaction(other);
    }
}
