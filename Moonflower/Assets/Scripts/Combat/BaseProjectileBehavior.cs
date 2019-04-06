using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileBehavior : MonoBehaviour, IProjectile
{
    public GameObject muzzleFlash;
    public GameObject explosion;
    public int baseDamage = 2;
    public float movementSpeed = 5f;
    public float lifetime = 3f;
    public float gravityStrength = 0.5f;
    private float timePassed;
    public bool affectedByGravity = false;

    public bool seeksTarget = false;
    [Range(0, 1)] public float seekStrength = 1f;

    public IHurtboxController Hurtbox { get; set; }
    public Transform TargetTransform { get; set; }

    void Awake()
    {
        Hurtbox = GetComponentInChildren<IHurtboxController>();
    }

    private void Start()
    {
        if (muzzleFlash)
        {
            ObjectPoolController.current.CheckoutTemporary(muzzleFlash, transform.position, 1);
        }
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > lifetime)
            OnHit(null);

        Vector3 velocity = Vector3.zero;

        if (seeksTarget)
        {
            //if (affectedByGravity)
            //    velocity = (TargetTransform.position - transform.position).normalized * movementSpeed + Physics.gravity;
            //else
            //    velocity = (TargetTransform.position - transform.position).normalized * movementSpeed;

            Quaternion lookRotation = Quaternion.LookRotation(TargetTransform.position - transform.position).normalized;
            float rotationDelta = Quaternion.Angle(transform.rotation, lookRotation) * 0.3f * seekStrength;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationDelta);

            if (affectedByGravity)
                velocity = (transform.forward * movementSpeed) + (Vector3.down * gravityStrength);
            else
                velocity = transform.forward * movementSpeed;
        }
        else
        {
            if (affectedByGravity)
                velocity = (transform.forward * movementSpeed) + (Vector3.down * gravityStrength);
            else
                velocity = transform.forward * movementSpeed;
        }

        transform.position += velocity * Time.deltaTime;
    }

    public void OnHit(GameObject other)
    {
        if (explosion)
        {
            ObjectPoolController.current.CheckoutTemporary(explosion, transform.position, 1);
        }

        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }
}
