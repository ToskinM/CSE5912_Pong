using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxxBall1 : MonoBehaviour
{
    public Vector3 velocity;

    private Vector3 startSpeed = new Vector3(5,1,0);
    private Vector3 startPosition = new Vector3(0, 0, 0); 
    private float bounceMultiplier = 1.05f;
    private float maxSpeed = 20f;
    private Rigidbody rigidBody;
    private GameStateManager gameStateManager;
    private AudioManager audioManager;

    void Start()
    {
        StartCoroutine(GetAudioManager());

        rigidBody = GetComponent<Rigidbody>();
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
        Spawn();
    }

    private IEnumerator GetAudioManager()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            yield return null;
        }
    }

    void Update()
    {

        //Debug.Log(velocity);
        if (gameStateManager.Paused)
        {
            rigidBody.velocity = new Vector3(0, 0, 0); 
            return;
        }

        transform.position += velocity * Time.deltaTime;
    }

    public void Spawn()
    {
        if (gameStateManager.ball != null)
        {
            gameStateManager.ball = gameObject;
        }

        transform.position = new Vector3(0, 0, 0);
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            velocity = startSpeed;
        }
        else
        {
            velocity = -1.0f * startSpeed; 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            //velocity = Vector3.Reflect(new Vector3(0f, Mathf.Clamp(bounceMultiplier * speed, -maxSpeed, maxSpeed), 0f), other.cont);
            //speed *= -bounceMultiplier;
            //speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        }
        if (other.gameObject.CompareTag("WinGoal"))
        {
            gameStateManager.Win();
            Spawn();
        }

        if (other.gameObject.CompareTag("LoseGoal"))
        {
            gameStateManager.Lose();
            Spawn(); 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Paddle"))
        {
            audioManager?.Play("Collision");
            Vector3 newVelocity;
            newVelocity = Vector3.ClampMagnitude(bounceMultiplier * Vector3.Reflect(velocity, collision.GetContact(0).normal), maxSpeed);
            newVelocity += collision.collider.gameObject.GetComponent<Rigidbody>().velocity;
            velocity = newVelocity;
                
        }

        if (collision.collider.gameObject.CompareTag("Wall"))
        {
            audioManager?.Play("Collision");
            Vector3 newVelocity;
            newVelocity = Vector3.ClampMagnitude(bounceMultiplier * Vector3.Reflect(velocity, collision.GetContact(0).normal), maxSpeed);
            newVelocity += collision.collider.gameObject.GetComponent<Rigidbody>().velocity;
            velocity = newVelocity;
        }


    }
}
