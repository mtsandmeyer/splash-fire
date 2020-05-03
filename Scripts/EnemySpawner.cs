using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTime = 3.0f;

    public float xMin = -25;
    public float xMax = 25;
    public float yMin = 8;
    public float yMax = 25;
    public float zMin = -25;
    public float zMax = 25;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);
    }

    void SpawnEnemies()
    {
        Vector3 enemyPosition;

        enemyPosition.x = Random.Range(xMin, xMax);
        enemyPosition.y = Random.Range(xMin, xMax);
        enemyPosition.z = Random.Range(zMin, zMax);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, enemyPosition, transform.rotation) as GameObject;

        spawnedEnemy.transform.parent = gameObject.transform;
    }
}
