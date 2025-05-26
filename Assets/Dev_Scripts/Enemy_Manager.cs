using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum enemyState { walking, stealing, running }

public class Enemy_Manager : MonoBehaviour
{

    [Header("AI")]
    private NavMeshAgent agent;
    

    [Header("States")]
    public int State;
    public bool isScared = false;

    [Header("Target")]
    public int whichBuildingInt;
    public GameObject[] Buildings;
    public float distanceToTarget;
    public float reachDistance = 1.0f;

    [Header("Stealing")]
    public float stealingTime;
    public float currentTime = 0;
    public int stolenResources;
    public int maxResourcesStolen;
    public int minResourcesStolen;

    [Header("Escaping")]
    public GameObject Spawn;

    [Header("Seeds")]
    private Enemy_Spawner spawner;
    public GameObject[] seeds;
    public float seedDropChance = 0.2f;

    [Header("Animations")]
    public Animator enemyAnimator;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Buildings = GameObject.FindGameObjectsWithTag("Building");
        Spawn = GameObject.Find("EnemySpawn");
        spawner = FindObjectOfType<Enemy_Spawner>();

        whichBuildingInt = Random.Range(0, Buildings.Length);

        State = 1;
    }

    public void Update()
    {
        if (State == 1)
        {
            ToPlot(Buildings[whichBuildingInt].transform.position);
        }

        if(State == 2)
        {
            Stealing();
        }

        if (State == 3)
        {
            Leave();
        }

        if (agent.isStopped == true)
        {
            enemyAnimator.SetBool("isRunning", false);
        }
        else
        {
            enemyAnimator.SetBool("isRunning", true);
        }
    }
    public void ToPlot(Vector3 destination)
    {
        if (!agent.hasPath)
        {
            agent.SetDestination(destination);
        }

        distanceToTarget = Vector3.Distance(agent.transform.position, destination);
        if (distanceToTarget < reachDistance)
        {
            agent.isStopped = true;
            State = 2;
        }
    }

    public void Stealing()
    {
        if (currentTime >= stealingTime && !spawner.isTutorial)
        {
            stolenResources = Random.Range(minResourcesStolen, maxResourcesStolen);
            if (Random.value > 0.5f)
            {
                ResourceManager.resourceManager.RemoveStone(stolenResources);
            }
            else
            {
                ResourceManager.resourceManager.RemoveWood(stolenResources);
            }

            State = 3;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    public void Scare()
    {
        if (isScared)
        {
            return;
        }
        currentTime = 0;
        isScared = true;
        DropSeed();
        spawner.CheckRaidCompletion();
        State = 3;
        Leave();
    }

    private void DropSeed()
    {
        if (spawner.firstSeed)
        {
            Instantiate(seeds[0], transform.position, Quaternion.Euler(-90, 0, 0));
            spawner.firstSeed = false;
        }
        else if (Random.value < seedDropChance && !FindObjectOfType<Enemy_Spawner>().isTutorial)
        {
            Instantiate(seeds[Random.Range(0, seeds.Length)], transform.position, Quaternion.Euler(-90, 0, 0));
        }
    }

    public void Leave()
    {
        agent.isStopped = false;
        agent.SetDestination(Spawn.transform.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Destroy(this.gameObject);
        }
    }
}