using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolController : MonoBehaviour
{
    public static ObjectPoolController current;
    public int poolCount;

    public Dictionary<string, ObjectPool> pools;

    private const int CAPACITY_DEFAULT = 10;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pools = new Dictionary<string, ObjectPool>();
    }

    void Update()
    {
        poolCount = pools.Count;
    }

    /// <summary>
    /// </summary>
    public GameObject Checkout(GameObject gameObject, Transform transform)
    {
        ObjectPool pool = null;
        if (pools.ContainsKey(gameObject.name) && pools.TryGetValue(gameObject.name, out pool))
        {
            return pool.Checkout(transform);
        }
        else
        {
            pool = new ObjectPool(gameObject, CAPACITY_DEFAULT);
            pools.Add(gameObject.name, pool);

            return pool.Checkout(transform);
        }
    }
    /// <summary>
    /// </summary>
    public GameObject CheckoutTemporary(GameObject gameObject, Transform transform, float delay)
    {
        GameObject request = null;
        ObjectPool pool = null;
        if (pools.ContainsKey(gameObject.name) && pools.TryGetValue(gameObject.name, out pool))
        {
            request = pool.Checkout(transform);
        }
        else
        {
            pool = new ObjectPool(gameObject, CAPACITY_DEFAULT);
            pools.Add(gameObject.name, pool);

            request = pool.CheckoutTemporary(transform, delay);
        }

        //request.transform.parent = transform;
        if (request != null)
            StartCoroutine(ReturnAfterDelay(request, delay));

        return request;
    }

    public GameObject CheckoutTemporary(GameObject gameObject, Vector3 position, float delay)
    {
        GameObject request = null;
        ObjectPool pool = null;
        if (pools.ContainsKey(gameObject.name) && pools.TryGetValue(gameObject.name, out pool))
        {
            request = pool.Checkout(position);
        }
        else
        {
            pool = new ObjectPool(gameObject, CAPACITY_DEFAULT);
            pools.Add(gameObject.name, pool);

            request = pool.CheckoutTemporary(position, delay);
        }

        //request.transform.parent = transform;
        if (request != null)
            StartCoroutine(ReturnAfterDelay(request, delay));

        return request;
    }

    private IEnumerator ReturnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj.activeInHierarchy)
            obj.SetActive(false);
    }
}