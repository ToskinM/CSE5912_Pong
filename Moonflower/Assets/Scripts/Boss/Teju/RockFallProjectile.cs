using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFallProjectile : MonoBehaviour, IProjectile
{
    public GameObject explosion;
    public int baseDamage = 2;
    public float lifetime = 3f;
    private float timePassed;

    public IHurtboxController Hurtbox { get; set; }
    public Transform TargetTransform { get; set; }

    void Awake()
    {
        Hurtbox = GetComponentInChildren<IHurtboxController>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(Random.Range(0.3f, 1) * transform.localScale.x, Random.Range(0.3f, 1) * transform.localScale.y, Random.Range(0.3f, 1) * transform.localScale.z);
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-5,5), Random.Range(-5, 5), Random.Range(-5, 5)), ForceMode.Impulse);
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > lifetime)
            OnHit(null);
    }

    public void OnHit(GameObject other)
    {
        if (explosion)
        {
            ObjectPoolController.current.CheckoutTemporary(explosion, transform.position, 2);
        }

        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }
}
