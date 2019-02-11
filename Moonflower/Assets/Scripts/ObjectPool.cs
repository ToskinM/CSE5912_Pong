using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //public static ObjectPoolController current;
    public GameObject prefabToPool;
    public int poolCapacity;

    public List<GameObject> pooledObjects;

    public ObjectPool(GameObject gameObject, int capacity)
    {
        prefabToPool = gameObject;
        poolCapacity = capacity;

        if (poolCapacity < 1)
        {
            Debug.Log("<color=red> Object Pool " + gameObject.name + " is set to zero capacity.</color>");
            return;
        }

        if (prefabToPool == null)
        {
            Debug.Log("<color=red> Object Pool " + gameObject.name + " has no prefab to set.</color>");
            return;
        }

        // Create Pool
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolCapacity; i++)
        {
            GameObject objectToAdd = Instantiate(prefabToPool);
            objectToAdd.SetActive(false);
            pooledObjects.Add(objectToAdd);
        }
    }

    void Start()
    {
        if (poolCapacity < 1)
        {
            Debug.Log("<color=red> Object Pool " + gameObject.name + " is set to zero capacity.</color>");
            return;
        }

        if (prefabToPool == null)
        {
            Debug.Log("<color=red> Object Pool " + gameObject.name + " has no prefab to set.</color>");
            return;
        }

        // Create Pool
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolCapacity; i++)
        {
            GameObject objectToAdd = Instantiate(prefabToPool);
            objectToAdd.SetActive(false);
            pooledObjects.Add(objectToAdd);
        }
    }

    /// <summary>Gets and returns an object from the pool if one is available. (without setting it active or setting its transfrom)
    /// </summary>
    public GameObject GetPooledObject()
    {
        // Get and return the first available obect in the pool
        for (int i = 0; i < pooledObjects.Count; i++)
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];

        return null;
    }

    /// <summary>Returns an object from the pool and matches its transfrom to the desired one.
    /// </summary>
    public GameObject Checkout(Transform transform)
    {
        // Grab an object from the pool
        GameObject objectFromPool = GetPooledObject();

        // Return null if we can get an object
        if (objectFromPool == null)
            return null;

        // Set the objects transfrom to match the desired transform, and set it active
        objectFromPool.transform.position = transform.position;
        objectFromPool.transform.rotation = transform.rotation;
        objectFromPool.SetActive(true);

        return objectFromPool;
    }

    /// <summary>Returns an object from the pool and matches only its position to the desired one.
    /// </summary>
    public GameObject Checkout(Vector3 position)
    {
        // Grab an object from the pool
        GameObject objectFromPool = GetPooledObject();

        // Return null if we can get an object
        if (objectFromPool == null)
            return null;

        // Set the objects position to match the desired position, and set it active
        objectFromPool.transform.position = position;
        objectFromPool.SetActive(true);

        return objectFromPool;
    }

    /// <summary>Returns an object from the pool and matches only its position to the desired one, returning it to the pool after the delay.
    /// </summary>
    public GameObject CheckoutTemporary(Vector3 position, float delay)
    {
        // Grab an object from the pool
        GameObject objectFromPool = GetPooledObject();

        // Return null if we can get an object
        if (objectFromPool == null)
            return null;

        // Set the objects position to match the desired position, and set it active
        objectFromPool.transform.position = position;
        objectFromPool.SetActive(true);

        //StartCoroutine(ReturnAfterDelay(objectFromPool, delay));

        return objectFromPool;
    }
    /// <summary>Returns an object from the pool and matches its transfrom to the desired one, returning it to the pool after the delay.
    /// </summary>
    public GameObject CheckoutTemporary(Transform transform, float delay)
    {
        // Grab an object from the pool
        GameObject objectFromPool = GetPooledObject();

        // Return null if we can get an object
        if (objectFromPool == null)
            return null;

        // Set the objects transfrom to match the desired transform, and set it active
        objectFromPool.transform.position = transform.position;
        objectFromPool.transform.rotation = transform.rotation;
        objectFromPool.SetActive(true);

        //StartCoroutine(ReturnAfterDelay(objectFromPool, delay));

        return objectFromPool;
    }

    private IEnumerator ReturnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj.activeInHierarchy)
            obj.SetActive(false);
    }
}