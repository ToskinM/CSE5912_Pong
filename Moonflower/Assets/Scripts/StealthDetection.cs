using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StealthDetection : MonoBehaviour
{
    public enum AwarenessLevel { Neutral, Suspicious, Alerted, Distracted };
    public AwarenessLevel Awareness;

    private NavMeshAgent nav;
    private SphereCollider col;
    public float suspicionMeter;
    private float timeToClearSuspicion = 3f;
    private float timeSinceLastAction;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        nav = GetComponent<NavMeshAgent>();

        timeSinceLastAction = 0f;
        Awareness = AwarenessLevel.Neutral;

        GameStateController.OnPaused += HandlePauseEvent;
    }

    // Update is called once per frame
    void Update()
    {
        // If some time has passed since last player movement, reduce suspicion levels
        if (timeSinceLastAction >= timeToClearSuspicion)
        {
            if (suspicionMeter > 0f) SuspicionDecay();
        }
        else
        {
            timeSinceLastAction += Time.deltaTime;  // Wait some time before the decay starts
        }

        Debug.Log(Awareness.ToString() + " , "  + timeSinceLastAction.ToString());
    }

    void DetectionWhileNeutral(GameObject player, float distance)
    {
        // If player is within half the radius of the detection range
        if (distance <= col.radius / 2)
        {
            // Add small amounts of suspicion if player is walking
            if (player.GetComponent<PlayerMovement>().Action == Actions.Walking)
            {
                suspicionMeter += 2.5f;
                timeSinceLastAction = 0f;
            }
            // Add bigger amounts of suspicion if player is running
            else if (player.GetComponent<PlayerMovement>().Action == Actions.Running)
            {
                suspicionMeter += 5f;
                timeSinceLastAction = 0f;
            }
        }
        // If player is within the detection range but is further than half its radius
        else if (distance <= col.radius && player.GetComponent<PlayerMovement>().Action == Actions.Running)
        {
            suspicionMeter += 3f;
            timeSinceLastAction = 0f;
        }

        // Once suspicion meter is past its threshold, turn into suspicious state
        if (suspicionMeter >= 100f)
        {
            StartCoroutine(BecomeSuspiciousAfterDelay(0f));
        }
        else if (suspicionMeter <= 0f)
        {
            StartCoroutine(BecomeNeutralAfterDelay(0f));
        }
    }

    void DetectionWhileSuspicious(GameObject player, float distance)
    {
        if (player.GetComponent<PlayerMovement>().Action == Actions.Walking)
        {
            suspicionMeter += 5f;
            timeSinceLastAction = 0f;
        }
        // Add bigger amounts of suspicion if player is running
        else if (player.GetComponent<PlayerMovement>().Action == Actions.Running)
        {
            suspicionMeter += 10f;
            timeSinceLastAction = 0f;
        }
        // Add smaller amounts of suspicion if player is sneaking
        else if (player.GetComponent<PlayerMovement>().Action == Actions.Sneaking)
        {
            suspicionMeter += 1f;
            timeSinceLastAction = 0f;
        }

        if (suspicionMeter >= 200f)
        {
            StartCoroutine(BecomeAlertedAfterDelay(2f, player));
        }
        else if (suspicionMeter <= 100f)
        {
            StartCoroutine(BecomeNeutralAfterDelay(0f));
        }
    }

    void DetectionWhileAlert(GameObject player, float distance)
    {
        Actions playerAction = player.GetComponent<PlayerMovement>().Action;
        if (playerAction == Actions.Walking || playerAction == Actions.Running)
        {
            nav.destination = player.transform.position;
            timeSinceLastAction = 0f;
        }

        if (suspicionMeter <= 100f)
        {
            StartCoroutine(BecomeNeutralAfterDelay(0f));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerMovement.NoiseRaised += HandleNoiseRaised;
    }

    void OnTriggerStay(Collider other)
    {
        if (!enabled) return;

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            float distance = CalculatePathLength(player.transform.position);

            if (Awareness == AwarenessLevel.Neutral)
            {
                DetectionWhileNeutral(player, distance);
            }
            else if (Awareness == AwarenessLevel.Suspicious)
            {
                DetectionWhileSuspicious(player, distance);
            }
            else if (Awareness == AwarenessLevel.Alerted)
            {
                DetectionWhileAlert(player, distance);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            StartCoroutine(BecomeNeutralAfterDelay(3f));
            PlayerMovement.NoiseRaised -= HandleNoiseRaised;
        }
    }

    float CalculatePathLength(Vector3 targetPosiion)
    {
        NavMeshPath path = new NavMeshPath();

        if (nav.enabled)
        {
            nav.CalculatePath(targetPosiion, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosiion;

        for (int i=0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for(int i=0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

    void HandleNoiseRaised()
    {
        if (Awareness == AwarenessLevel.Neutral)
        {
            suspicionMeter += 25f;
        }
        if (Awareness == AwarenessLevel.Suspicious)
        {
            suspicionMeter += 50f;
        }
        timeSinceLastAction = 0f;
    }

    void SuspicionDecay()
    {
        suspicionMeter -= 1f;
    }

    IEnumerator BecomeAlertedAfterDelay(float time, GameObject player)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        Vector3 detectedPosition = player.transform.position;
        yield return new WaitForSeconds(time);
        Awareness = AwarenessLevel.Alerted;
        nav.destination = detectedPosition;
    }

    IEnumerator BecomeSuspiciousAfterDelay(float time)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(time);
        Awareness = AwarenessLevel.Suspicious;
    }

    IEnumerator BecomeNeutralAfterDelay(float time)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(time);
        Awareness = AwarenessLevel.Neutral;
    }

    IEnumerator BecomeDistracted(float time)
    {
        yield return new WaitForSeconds(time);
    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }
}
