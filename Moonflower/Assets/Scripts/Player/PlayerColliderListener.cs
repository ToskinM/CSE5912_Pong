using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderListener : MonoBehaviour
{
    private TerrainCollider terrain;

    public delegate void TerrainCollision();
    public static event TerrainCollision CollideWithTerrain;

    public delegate void HurtboxCollision(Collider hurtboxSource);
    public static event HurtboxCollision OnHurtboxHit;

    void Start()
    {
        terrain = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return; // Don't do anything if this game object is not the active player object

        if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5)
        {
            CollideWithTerrain?.Invoke();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if (other.tag == "Hurtbox" || other.tag == "EnvironmentHurtbox")
        {
            //Debug.Log("enter hit");
            OnHurtboxHit?.Invoke(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;

        if (other.tag == "EnvironmentHurtbox")
        {
            //Debug.Log("stay hit");
            OnHurtboxHit?.Invoke(other);
        }
    }
}
