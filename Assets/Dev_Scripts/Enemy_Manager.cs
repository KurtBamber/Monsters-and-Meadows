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
        Buildings = GameObject.FindGameObjectsWithTag("EmptyPlot");
        Spawn = GameObject.Find("Enemy Spawn");

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
        agent.SetDestination(destination);

        distanceToTarget = Vector3.Distance(agent.transform.position, destination);
        if (distanceToTarget < reachDistance)
        {
            agent.isStopped = true;
            State = 2;
        }
    }

    public void Stealing()
    {
        if(currentTime >= stealingTime)
        {
            stolenResources = Random.Range(minResourcesStolen, maxResourcesStolen);
            State = 3;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    public void Leave()
    {
        agent.isStopped = false;
        agent.SetDestination(Spawn.transform.position);


        if (agent.remainingDistance <= 10)
        {
            Destroy(this.gameObject);
        }
        
    }

}
