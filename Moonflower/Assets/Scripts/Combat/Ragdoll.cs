using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private const float defaultFreezeDelay = 10f;
    private const float defaultDestroyDelay = 15f;

    private void Start()
    {
        StartCoroutine(FreezeRagdoll(defaultFreezeDelay, defaultDestroyDelay));
    }

    // Get joints from the character and match their position (match characters pose)
    // Should be called by the instantiating object immediatley
    public void MatchPose(Transform[] joints)
    {
        Transform[] ragdollJoints = gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < ragdollJoints.Length; i++)
        {
            for (int q = 0; q < joints.Length; q++)
            {
                if (joints[q].name.CompareTo(ragdollJoints[i].name) == 0)
                {
                    ragdollJoints[i].position = joints[q].position;
                    ragdollJoints[i].rotation = joints[q].rotation;
                    break;
                }
            }
        }
    }

    // Freeze this ragdoll after a delay, then delay destroy it
    private IEnumerator FreezeRagdoll(float freezeDelay, float destroyDelay)
    {
        yield return new WaitForSeconds(freezeDelay);

        Collider[] ragdollColliders = gameObject.GetComponentsInChildren<Collider>();
        Rigidbody[] ragdollRigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = false;
        }
        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].collisionDetectionMode = CollisionDetectionMode.Discrete;
            ragdollRigidbodies[i].isKinematic = true;
        }

        //StartCoroutine(FadeAndDestroy());
        Destroy(gameObject, destroyDelay);
    }

    //private IEnumerator FadeAndDestroy()
    //{
    //    SkinnedMeshRenderer[] ragdollRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    //    Debug.Log(ragdollRenderers.Length);

    //    while (ragdollRenderers[0].material.color.a > 0)
    //    {
    //        for (int i = 0; i < ragdollRenderers.Length; i++)
    //        {
    //            Material material = ragdollRenderers[i].material;
    //            material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a - (Time.deltaTime));
    //        }

    //        yield return null;
    //    }

    //    Destroy(gameObject, 1f);
    //}
}
