using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolController : MonoBehaviour
{
    public static ObjectPoolController current;
    public int poolCount;

    public Dictionary<string, ObjectPool> pools;

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

    void CreateNewPool(ObjectPool pool)
    {
        pool = new ObjectPool(gameObject, 1);
        pools.Add(gameObject.name, pool);
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
            pool = new ObjectPool(gameObject, 1);
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
            pool = new ObjectPool(gameObject, 1);
            pools.Add(gameObject.name, pool);

            request = pool.CheckoutTemporary(transform, delay);
        }

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