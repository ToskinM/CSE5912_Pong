using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TejuCrystalScript : MonoBehaviour
{
    public GameObject hitEffect;
    public int health = 5;

    private float hurtdelay = 0.2f;
    private float timeSinceLastHurt;

    void Start()
    {
        
    }

    void Update()
    {
        timeSinceLastHurt += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHurtbox")
        {
            if (timeSinceLastHurt >= hurtdelay)
            {
                timeSinceLastHurt = 0f;

                ObjectPoolController.current.CheckoutTemporary(hitEffect, transform, 1);

                IHurtboxController hurtboxController = other.gameObject.GetComponent<IHurtboxController>();
                GameObject source = hurtboxController.Source;
                health -= hurtboxController.Damage;

                Debug.Log(gameObject.name + " took <color=red>" + hurtboxController.Damage + "</color> damage");
            }
        }
    }
}
