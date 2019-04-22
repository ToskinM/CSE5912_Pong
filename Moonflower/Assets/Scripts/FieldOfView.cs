using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    private float startingViewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public NPCCombatController npcCombatController;

    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();
    [HideInInspector] public Transform closestTarget = null;
    [HideInInspector] public Transform focusedTarget = null;

    private Material emptyMaterial;
    private Material foundMaterial;
    private float meshResolution = 1;
    private int edgeResolveIterations = 1;
    private float edgeDstThreshold = 1;

    private MeshFilter viewMeshFilter;
    private bool drawFOVMesh;
    private Mesh viewMesh;

    public delegate void ClosestTargetUpdate(GameObject newClosestTarget);
    public event ClosestTargetUpdate OnNewClosestTarget;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == Constants.SCENE_CAVEBOSS)
        {
            viewRadius *= 2;
        }

        startingViewAngle = viewAngle;

        viewMeshFilter = Instantiate(Resources.Load<GameObject>("Debug/FieldOfViewMesh"), transform).GetComponent<MeshFilter>();
        foundMaterial = Resources.Load<Material>("Materials/TriggerRed");
        emptyMaterial = Resources.Load<Material>("Materials/TriggerYellow");

        if (viewMeshFilter)
        {
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;
        }

        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    public void Reset()
    {
        startingViewAngle = viewAngle;
        closestTarget = null;
        focusedTarget = null;
        StartCoroutine("FindTargetsWithDelay", 0.2f);

    }

    private void OnEnable()
    {
        GameStateController.OnDebugViewToggle += HandleDebugViewToggle;
    }
    private void OnDisable()
    {
        GameStateController.OnDebugViewToggle -= HandleDebugViewToggle;
    }

    public void SetAggressionMode()
    {
        NPCCombatController.Aggression aggression = npcCombatController.aggression;

        switch (aggression)
        {
            case NPCCombatController.Aggression.Passive:
                targetMask = 0;
                break;
            case NPCCombatController.Aggression.Unaggressive:
                targetMask |= 1 << LayerMask.NameToLayer("Player");
                break;
            case NPCCombatController.Aggression.Aggressive:
                targetMask |= 1 << LayerMask.NameToLayer("Player");
                break;
            case NPCCombatController.Aggression.Frenzied:
                targetMask |= 1 << LayerMask.NameToLayer("NPC");
                targetMask |= 1 << LayerMask.NameToLayer("Player");
                break;
            default:
                break;
        }
    }

    public void SetCombatMode(bool combatMode)
    {
        if (combatMode)
        {
            viewAngle = Mathf.Clamp(startingViewAngle * 2, 0, 360);
        }
        else
        {
            viewAngle = Mathf.Clamp(startingViewAngle, 0, 360);
        }
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            if (npcCombatController)
            {
                SetAggressionMode();
            }

            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            GetClosestTarget();
            GetFocusedTarget();

            if (viewMeshFilter)
                if (visibleTargets.Count > 0)
                    viewMeshFilter.GetComponent<Renderer>().material = foundMaterial;
                else
                    viewMeshFilter.GetComponent<Renderer>().material = emptyMaterial;
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
            if (target.gameObject != gameObject && Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
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

            if (closest != closestTarget)
            {
                closestTarget = closest;
                //OnNewClosestTarget?.Invoke(closestTarget.gameObject);
                OnNewClosestTarget?.Invoke(LesserNPCController.GetRootmostObjectInLayer(closestTarget, "NPC"));
            }
        }
        else
        {
            closestTarget = null;
            OnNewClosestTarget?.Invoke(null);
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

    public bool IsInFieldOfView(Transform transform)
    {
        if (transform == null)
            return false;

        for (int i = 0; i < visibleTargets.Count; i++)
            if (visibleTargets[i] == transform)
                return true;

        return false;
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void LateUpdate()
    {
        if (viewMeshFilter && drawFOVMesh)
            DrawFieldOfView();
    }

    private void HandleDebugViewToggle(bool debugViewOn)
    {
        drawFOVMesh = debugViewOn;
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}