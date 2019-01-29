using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public GameObject ball;
    private float speed = 9f;
    private GameStateManager gameStateManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetGameStateManager());
    }

    private IEnumerator GetGameStateManager()
    {
        while (gameStateManager == null)
        {
            gameStateManager = GameObject.Find("Game State Manager").GetComponent<GameStateManager>();
            yield return null;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float deltaX = (ball.transform.position - transform.position).y;
        Vector3 targetLocation = new Vector3(transform.position.x, ball.transform.position.y, transform.position.z);
        Vector3 temp = Vector3.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);
        transform.position = gameStateManager.OutOfBounds(temp);
    }
}
