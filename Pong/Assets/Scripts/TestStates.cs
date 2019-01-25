using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStates : MonoBehaviour
{
    private GameStateManager gameStateManager;
    public MaxxBall1 ball;
    void Start()
    {
        gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
        ball = GameObject.Find("MaxxBall1").GetComponent<MaxxBall1>();
        Debug.Log(ball.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z)) {
            ball.velocity = new Vector3(5, 1, 0);
            ball.transform.position = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.X))
        {
            ball.velocity = new Vector3(-5, -1, 0);
            ball.transform.position = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.C))
        {
            ball.velocity = new Vector3(20, 1, 0);
            ball.transform.position = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.V))
        {
            ball.velocity = new Vector3(-20, -1, 0);
            ball.transform.position = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.B))
        {
            ball.velocity = new Vector3(4, 8, 0);
            ball.transform.position = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.N))
        {
            ball.velocity = new Vector3(-4, -8, 0);
            ball.transform.position = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            gameStateManager.Lose();
            ball.velocity = new Vector3(5, 1, 0);
            ball.transform.position = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyUp(KeyCode.Comma))
        {
            gameStateManager.Win();
            ball.velocity = new Vector3(5, 1, 0);
            ball.transform.position = new Vector3(0, 0, 0);
        }
    }
}
