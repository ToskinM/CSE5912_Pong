using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxxBall1 : MonoBehaviour
{
    private Vector3 startSpeed = new Vector3(1,3,0);
    private float bounceMultiplier = 1.05f;
    private float maxSpeed = 20f;
    private Vector3 velocity;
    private Rigidbody rigidbody;
    private GameStateManager gameStateManager;

    void Start()
    {

        rigidbody = GetComponent<Rigidbody>();
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
        velocity = startSpeed;
    }

    void Update()
    {
        Debug.Log(velocity);
        if (gameStateManager.Paused) return;

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

        if (collision.collider.gameObject.CompareTag("Wall"))
        {
            Vector3 newVelocity;
            newVelocity = Vector3.ClampMagnitude(bounceMultiplier * Vector3.Reflect(velocity, collision.GetContact(0).normal), maxSpeed);
            newVelocity += collision.collider.gameObject.GetComponent<Rigidbody>().velocity;
            velocity = newVelocity;
        }

        if (collision.collider.gameObject.CompareTag("WinGoal"))
        {

        }
    }
}
