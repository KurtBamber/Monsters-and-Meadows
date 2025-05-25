using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public int minAmountSpawned, maxAmountSpawned, amountToSpawn;
    public float minRaidDelay, maxRaidDelay;
    public GameObject enemyPrefab;
    public bool spawnEnemies, enemiesHaveSpawned, isTutorial;
    public LayerMask enemyLayer;
    public bool firstRaid = true;

    public void Update()
    {
        if (spawnEnemies && !enemiesHaveSpawned && ResourceManager.resourceManager.totalStone > 0 && ResourceManager.resourceManager.totalWood > 0)
        {
            if (!isTutorial)
            {
                amountToSpawn = Random.Range(minAmountSpawned, maxAmountSpawned);
            }
            else
            {
                amountToSpawn = 2;
            }
            SpawnEnemies();
            enemiesHaveSpawned = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayer))
            {
                Enemy_Manager target = hit.collider.GetComponent<Enemy_Manager>();
                if (target != null && !target.isScared)
                {
                    MonsterAI nearestMonster = FindNearestMonster(target.transform.position);
                    if (nearestMonster != null)
                    {
                        nearestMonster.ScareTarget(target, 25f);
                    }
                }
            }
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
        }
        StartCoroutine(RaidWait());
    }

    MonsterAI FindNearestMonster(Vector3 targetPosition)
    {
        MonsterAI[] allMonsters = FindObjectsOfType<MonsterAI>();
        MonsterAI nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var monster in allMonsters)
        {
            if (monster.CanScare())
            {
                float dist = Vector3.Distance(monster.transform.position, targetPosition);
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    nearest = monster;
                }
            }
        }

        return nearest;
    }

    public void CheckRaidCompletion()
    {
        if (!firstRaid) return;

        Enemy_Manager[] allEnemies = FindObjectsOfType<Enemy_Manager>();
        foreach (var enemy in allEnemies)
        {
            if (!enemy.isScared)
            {
                return;
            }
        }

        firstRaid = false;
        FindObjectOfType<DialogueTrigger>().OnRaidComplete();
    }

    private IEnumerator RaidWait()
    {
        if (!isTutorial)
        {
            float waitTime = Random.Range(minRaidDelay, maxRaidDelay);
            yield return new WaitForSeconds(waitTime);
            enemiesHaveSpawned = false;
        }
        else
        {
            yield return null;
        }
    }

}