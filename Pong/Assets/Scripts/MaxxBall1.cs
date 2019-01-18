using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxxBall1 : MonoBehaviour
{
    public float speed = 3f;
    public float bounceMultiplier = 2f;
    public float maxSpeed = 20f;
    public Vector3 velocity;

    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        velocity = new Vector3(0f, speed, 0f);
    }

    void Update()
    {

        transform.position += velocity * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {

            //velocity = Vector3.Reflect(new Vector3(0f, Mathf.Clamp(bounceMultiplier * speed, -maxSpeed, maxSpeed), 0f), other.cont);
            //speed *= -bounceMultiplier;
            //speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Paddle"))
        {
            Vector3 newVelocity;
            newVelocity = Vector3.ClampMagnitude(bounceMultiplier * Vector3.Reflect(velocity, collision.GetContact(0).normal), maxSpeed);
            newVelocity += collision.collider.gameObject.GetComponent<Rigidbody>().velocity;
            velocity = newVelocity;
                
        }
    }
}
