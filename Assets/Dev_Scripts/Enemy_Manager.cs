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
    public enemyState currentState;
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


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Buildings = GameObject.FindGameObjectsWithTag("Building");
        Spawn = GameObject.Find("Spawn");

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
        if (currentTime >= stealingTime && !FindObjectOfType<Enemy_Spawner>().isTutorial)
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
        State = 3;
        isScared = true;
    }

    public void Leave()
    {
        agent.SetDestination(Spawn.transform.position);
        agent.isStopped = false;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Destroy(this.gameObject);
        }
    }


}
