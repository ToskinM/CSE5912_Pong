using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float timeUntilFall = 2;
    public float timeTillResurface = 3;
    public float fallTime = 1;
    public float fallDistance = 3;
    public bool playerTriggerOnly;

    private enum PlatformState { Idle, Falling, Fallen, Rising };
    private PlatformState state = PlatformState.Idle;

    private float fallCountdownTimer;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (state == PlatformState.Idle)
        {
            if (fallCountdownTimer >= timeUntilFall)
            {
                StartCoroutine(Fall());
            }
        }
        else if (state == PlatformState.Fallen)
        {
            if (fallCountdownTimer >= timeTillResurface)
                StartCoroutine(Rise());
            else
                fallCountdownTimer += Time.deltaTime;
        }
    }

    private IEnumerator Fall()
    {
        state = PlatformState.Falling;

        Vector3 fallenPosition = startingPosition - new Vector3(0, fallDistance, 0);

        float time = 0;
        while (time < fallTime)
        {
            transform.position = Vector3.Lerp(startingPosition, fallenPosition, time / fallTime);
            transform.position += new Vector3(Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f));
            time += Time.deltaTime;
            yield return null;
        }

        state = PlatformState.Fallen;
        fallCountdownTimer = 0;
    }

    private IEnumerator Rise()
    {
        state = PlatformState.Rising;

        Vector3 fallenPosition = transform.position;

        float time = 0;
        while (time < fallTime)
        {
            transform.position = Vector3.Lerp(fallenPosition, startingPosition, time / fallTime);
            time += Time.deltaTime;
            yield return null;
        }

        state = PlatformState.Idle;
        fallCountdownTimer = 0;
    }
    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnTriggerStay(Collider other)
    {
        //if (state == PlatformState.Idle)
        //{
        //    fallCountdownTimer += Time.deltaTime;
        //    transform.position = startingPosition + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        //}
        if (state == PlatformState.Idle)
        {
            if (playerTriggerOnly)
            {
                if (LesserNPCController.GetRootmostObjectInLayer(other.transform, "Player") == PlayerController.instance.GetActivePlayerObject())
                {
                    fallCountdownTimer += Time.deltaTime;
                    transform.position = startingPosition + new Vector3(Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f));
                }
            }

            else
            {
                fallCountdownTimer += Time.deltaTime;
                transform.position = startingPosition + new Vector3(Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (state == PlatformState.Idle)
        {
            if (playerTriggerOnly)
            {
                if (LesserNPCController.GetRootmostObjectInLayer(other.transform, "Player") == PlayerController.instance.GetActivePlayerObject())
                    fallCountdownTimer = 0;
            }
            else
                fallCountdownTimer = 0;
        }
    }
}
