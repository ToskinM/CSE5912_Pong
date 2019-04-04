using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StealthDetection : MonoBehaviour
{
    public enum AwarenessLevel { Distracted, Neutral, Suspicious, Alerted };
    public AwarenessLevel Awareness;

    public delegate void AwarenessUpdate(int newAwareness);
    public event AwarenessUpdate OnAwarenessUpdate;

    [HideInInspector] public float awarenessMeter;
    public float sensitivity = 1f;
    public const float awarenessMeterMax = 1000f;

    public const float suspiciousThreshold = 500f;
    public const float alertedThreshold = 1000f;

    private const float baseAwarenessGrowth = 5f;
    private const float sneakingMultiplier = 0.5f;
    private const float runningMultiplier = 2f;
    private const float suspiciousMultiplier = 2f;

    private const float timeTillSuspicionDecay = 3f;
    private const float baseDecayRate = 10f;
    private const float decayMultiplierSuspicious = 0.5f;
    private const float decayMultiplierAlerted = 0.3f;

    private NavMeshAgent nav;
    public SphereCollider stealthCollider;
    private Renderer renderer;
    private ICombatController npcCombatController;
    private GameObject indicator;

    private float timeSinceLastAction = 0f;

    private void Awake()
    {
        //stealthCollider = GetComponent<SphereCollider>();
        nav = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        npcCombatController = GetComponent<ICombatController>();
    }

    void Start()
    {
        // Initialize Awareness Level
        switch (Awareness)
        {
            case AwarenessLevel.Distracted:
                BecomeNeutral();
                break;
            case AwarenessLevel.Neutral:
                BecomeNeutral();
                break;
            case AwarenessLevel.Suspicious:
                BecomeSuspicious();
                break;
            case AwarenessLevel.Alerted:
                BecomeSuspicious();
                break;
            default:
                BecomeNeutral();
                break;
        }
    }

    private void OnEnable()
    {
        GameStateController.OnPaused += HandlePauseEvent;
    }
    private void OnDisable()
    {
        GameStateController.OnPaused -= HandlePauseEvent;
    }

    void Update()
    {
        // If some time has passed since last player movement, reduce suspicion levels
        if (timeSinceLastAction >= timeTillSuspicionDecay)
        {
            if (awarenessMeter > 0f) SuspicionDecay();
        }
        else
        {
            timeSinceLastAction += Time.deltaTime;  // Wait some time before the decay starts
        }

        // Update meter value
        UpdateAwareness();
    }

    void UpdateAwareness()
    {
        awarenessMeter = Mathf.Clamp(awarenessMeter, 0, awarenessMeterMax);

        if (awarenessMeter >= alertedThreshold)
        {
            // When alert threshold crossed, become alerted if not already
            if (Awareness == AwarenessLevel.Suspicious || Awareness == AwarenessLevel.Neutral)
            {
                BecomeAlerted(LevelManager.current.currentPlayer);
            }
        }
        else if (awarenessMeter >= suspiciousThreshold)
        {
            // When suspicious threshold crossed, become suspicious if not already
            if (Awareness == AwarenessLevel.Neutral)
            {
                BecomeSuspicious();
            }
            else if (Awareness == AwarenessLevel.Alerted)
            {
                BecomeSuspicious();
            }
        }
        else if (awarenessMeter == 0)
        {
            // return to neutral once meter is empty
            if (Awareness == AwarenessLevel.Suspicious || Awareness == AwarenessLevel.Alerted)
            {
                BecomeNeutral();
            }
        }
        else if (awarenessMeter < suspiciousThreshold)
        {
            // return to suspicious state if alerted when we drop below susp threshold
            if (Awareness == AwarenessLevel.Alerted)
            {
                BecomeSuspicious();
            }
        }
 
    }

    private void RaiseAwarenessMeter(float amount)
    {
        awarenessMeter += amount * sensitivity; // might add stealth mod here
        timeSinceLastAction = 0f;
    }
    private void LowerAwarenessMeter(float amount)
    {
        awarenessMeter -= amount;
    }

    void DetectionWhileNeutral(GameObject player, float distance)
    {
        // If player is within half the radius of the detection range
        if (distance <= stealthCollider.radius * 0.7)
        {
            // Add small amounts of suspicion if player is walking
            if (PlayerController.instance.ActivePlayerMovementControls.Action == PlayerMovementController.Actions.Walking)
            {
                RaiseAwarenessMeter(baseAwarenessGrowth);
            }
            // Add bigger amounts of suspicion if player is running
            else if (PlayerController.instance.ActivePlayerMovementControls.Action == PlayerMovementController.Actions.Running)
            {
                RaiseAwarenessMeter(baseAwarenessGrowth * runningMultiplier);
            }
        }
        // If player is within the detection range but is further than half its radius
        else if (distance <= stealthCollider.radius && PlayerController.instance.ActivePlayerMovementControls.Action == PlayerMovementController.Actions.Running)
        {
            RaiseAwarenessMeter(baseAwarenessGrowth);
        }
    }

    void DetectionWhileSuspicious(GameObject player, float distance)
    {
        if (PlayerController.instance.ActivePlayerMovementControls.Action == PlayerMovementController.Actions.Walking)
        {
            RaiseAwarenessMeter(baseAwarenessGrowth * suspiciousMultiplier);
        }
        // Add bigger amounts of suspicion if player is running
        else if (PlayerController.instance.ActivePlayerMovementControls.Action == PlayerMovementController.Actions.Running)
        {
            RaiseAwarenessMeter(baseAwarenessGrowth * runningMultiplier * suspiciousMultiplier);
        }
        // Add smaller amounts of suspicion if player is sneaking
        else if (PlayerController.instance.ActivePlayerMovementControls.Action == PlayerMovementController.Actions.Sneaking)
        {
            RaiseAwarenessMeter(baseAwarenessGrowth * sneakingMultiplier);
        }
    }

    void DetectionWhileAlert(GameObject player, float distance)
    {
        PlayerMovementController.Actions playerAction = PlayerController.instance.ActivePlayerMovementControls.Action;
        if ((int)npcCombatController.AggressionLevel > 1 && !npcCombatController.InCombat)
        {
            if (playerAction == PlayerMovementController.Actions.Walking || playerAction == PlayerMovementController.Actions.Running)
            {
                nav.destination = player.transform.position;
                timeSinceLastAction = 0f;
            }
        }
    }


    // Trigger MEthods
    public void TriggerEnterReaction(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerMovement.NoiseRaised += HandleNoiseRaised;
    }

    public void TriggerStayReaction(Collider other)
    {
        if (!enabled) return;

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            float distance = CalculatePathLength(player.transform.position);

            // If player is breathing down their neck, detect em
            if (distance <= 2.5f)
            {
                BecomeAlerted(LevelManager.current.currentPlayer);
            }
            else
            {
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
    }

    public void TriggerExitReaction(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (renderer)
                renderer.material.color = Color.white;

            //StartCoroutine(BecomeNeutralDelayed(3f));

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

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

    void HandleNoiseRaised()
    {
        if (enabled)
        {
            if (Awareness == AwarenessLevel.Neutral)
            {
                awarenessMeter += 25f;
            }
            if (Awareness == AwarenessLevel.Suspicious)
            {
                awarenessMeter += 50f;
            }
            timeSinceLastAction = 0f;
        }
    }

    void SuspicionDecay()
    {
        if (Awareness == AwarenessLevel.Neutral)
        {
            LowerAwarenessMeter(baseDecayRate);
        }
        else if (Awareness == AwarenessLevel.Suspicious)
        {
            LowerAwarenessMeter(baseDecayRate * decayMultiplierSuspicious);
        }
        else if (Awareness == AwarenessLevel.Alerted)
        {
            LowerAwarenessMeter(baseDecayRate * decayMultiplierAlerted);
        }

        awarenessMeter = Mathf.Clamp(awarenessMeter, 0, 1000);
    }

    public void BecomeAlerted(GameObject player)
    {
        if (renderer)
            renderer.material.color = Color.red;

        Awareness = AwarenessLevel.Alerted;
        SpawnIndicator();
        if (awarenessMeter < alertedThreshold)
            awarenessMeter = alertedThreshold;
        OnAwarenessUpdate?.Invoke(3);

        if ((int)npcCombatController.AggressionLevel > 1)
        {
            Vector3 detectedPosition = player.transform.position + new Vector3(Random.Range(-2, 2), 0f, Random.Range(-2, 2));
            nav.destination = detectedPosition;
        }
    }
    IEnumerator BecomeAlertedDelayed(float time, GameObject player)
    {
        Vector3 detectedPosition = player.transform.position + new Vector3(Random.Range(-2, 2), 0f, Random.Range(-2, 2));

        yield return new WaitForSeconds(time);

        if (renderer)
            renderer.material.color = Color.red;

        Awareness = AwarenessLevel.Alerted;
        SpawnIndicator();
        if (awarenessMeter < alertedThreshold)
            awarenessMeter = alertedThreshold;

        OnAwarenessUpdate?.Invoke(3);

        if ((int)npcCombatController.AggressionLevel > 1)
        {
            nav.destination = detectedPosition;
        }
    }

    private void BecomeSuspicious()
    {
        if (renderer)
            renderer.material.color = Color.yellow;

        Awareness = AwarenessLevel.Suspicious;
        SpawnIndicator();
        if (awarenessMeter < suspiciousThreshold)
            awarenessMeter = suspiciousThreshold;
        OnAwarenessUpdate?.Invoke(2);
    }
    IEnumerator BecomeSuspiciousDelayed(float time)
    {
        yield return new WaitForSeconds(time);

        if (renderer)
            renderer.material.color = Color.yellow;

        Awareness = AwarenessLevel.Suspicious;
        SpawnIndicator();

        if (awarenessMeter < suspiciousThreshold)
            awarenessMeter = suspiciousThreshold;

        OnAwarenessUpdate?.Invoke(2);
    }

    private void BecomeNeutral()
    {
        if (renderer)
            renderer.material.color = Color.green;

        Awareness = AwarenessLevel.Neutral;
        SpawnIndicator();
        awarenessMeter = 0;
        OnAwarenessUpdate?.Invoke(1);
    }
    IEnumerator BecomeNeutralDelayed(float time)
    {
        yield return new WaitForSeconds(time);

        if (renderer)
            renderer.material.color = Color.green;

        Awareness = AwarenessLevel.Neutral;
        SpawnIndicator();
        OnAwarenessUpdate?.Invoke(1);
    }

    IEnumerator BecomeDistractedDelayed(float time)
    {
        yield return new WaitForSeconds(time);
        OnAwarenessUpdate?.Invoke(0);
    }

    private void SpawnIndicator()
    {
        if (indicator != null)
        {
            Destroy(indicator);
            indicator = null;
        }

        switch (Awareness)
        {
            case AwarenessLevel.Distracted:
                indicator = Instantiate((GameObject)Resources.Load("Icons/DistractedIndicator"), transform);
                indicator.transform.position += new Vector3(0, 2, 0);
                break;
            case AwarenessLevel.Neutral:
                break;
            case AwarenessLevel.Suspicious:
                indicator = Instantiate((GameObject)Resources.Load("Icons/SuspiciousIndicator"), transform);
                indicator.transform.position += new Vector3(0, 2, 0);
                //Destroy(indicator, 3);
                break;
            case AwarenessLevel.Alerted:
                indicator = Instantiate((GameObject)Resources.Load("Icons/AlertedIndicator"), transform);
                indicator.transform.position += new Vector3(0, 2, 0);
                //Destroy(indicator, 3);
                break;
            default:
                break;
        }
    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }
}
