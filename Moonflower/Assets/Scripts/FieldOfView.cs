using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();
    [HideInInspector] public Transform closestTarget = null;
    [HideInInspector] public Transform focusedTarget = null;

    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            GetClosestTarget();
            GetFocusedTarget();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void GetClosestTarget()
    {
        closestTarget = null;
        if (visibleTargets.Count > 0)
        {
            Transform closest = visibleTargets[0];
            float closestDistance = Vector3.Distance(visibleTargets[0].position, gameObject.transform.position);
            for (int i = 1; i < visibleTargets.Count; i++)
            {
                float distance = Vector3.Distance(visibleTargets[i].position, gameObject.transform.position);
                if (distance < closestDistance)
                {
                    closest = visibleTargets[i];
                    closestDistance = distance;
                }
            }
            closestTarget = closest;
        }
    }

    void GetFocusedTarget()
    {
        focusedTarget = null;
        Vector3 cameraDirection = gameObject.transform.forward;
        if (visibleTargets.Count > 0)
        {
            Transform closest = visibleTargets[0];
            float closestAngle = Vector3.Angle(visibleTargets[0].transform.position - gameObject.transform.position, cameraDirection);
            for (int i = 1; i < visibleTargets.Count; i++)
            {
                float angle = Vector3.Angle(visibleTargets[i].transform.position - gameObject.transform.position, cameraDirection);
                if (angle < closestAngle)
                {
                    closest = visibleTargets[i];
                    closestAngle = angle;
                }
            }
            focusedTarget = closest;
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}