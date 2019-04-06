using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TejuCrystalScript : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject shatterEffect;
    public Transform shatterEffectNode;
    public int health = 5;

    private bool shattered;

    private readonly float hurtdelay = 0.2f;
    private float timeSinceLastHurt;

    public delegate void ShatterUpdate();
    public event ShatterUpdate OnShatter;

    void Start()
    {
        
    }

    void Update()
    {
        timeSinceLastHurt += Time.deltaTime;

        if (!shattered && health <= 0)
        {
            shattered = true;
            Shatter();
        }
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

    private void Shatter()
    {
        OnShatter?.Invoke();
        Instantiate(shatterEffect, shatterEffectNode.position, shatterEffectNode.rotation);
        gameObject.SetActive(false);
        Destroy(gameObject, 1);
    }
}
