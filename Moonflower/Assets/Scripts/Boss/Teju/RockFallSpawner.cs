using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFallSpawner : MonoBehaviour
{
    public GameObject rockFallProjectile;
    public int rocksToSpawn = 3;
    public float spawnSpeed = 0.8f;
    public float spawnSpeedChaos = 0.5f;

    IEnumerator Start()
    {
        // Spawn rocks
        for (int i = 0; i < rocksToSpawn; i++)
        {
            Instantiate(rockFallProjectile, gameObject.transform);
            yield return new WaitForSeconds(spawnSpeed + Random.Range(-spawnSpeedChaos, spawnSpeedChaos));
        }

        // Cleanup self
        Destroy(gameObject, 2f);
    }
}
