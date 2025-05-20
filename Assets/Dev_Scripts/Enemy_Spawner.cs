using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public int minAmountSpawned, maxAmountSpawned, amountToSpawn;
    public GameObject enemyPrefab;
    public bool spawnEnemies, enemiesHaveSpawned;

    public void Start()
    {
        amountToSpawn = Random.Range(minAmountSpawned, maxAmountSpawned);
    }

    public void Update()
    {
        if(spawnEnemies && !enemiesHaveSpawned)
        {
            SpawnEnemies();
            enemiesHaveSpawned = true;
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
        }
    }

}
