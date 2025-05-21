using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static MonsterType;

public enum monsterState { idle, wandering, following, mining, chopping, building }

public class MonsterAI : MonoBehaviour
{
    [Header("AI Navigation")]
    public NavMeshAgent agent;
    private WanderBehaviour wanderBehaviour;
    private MiningBehaviour miningBehaviour;
    private BuildingBehaviour buildingBehaviour;
    private ChoppingBehaviour choppingBehaviour;


    [Header("Abilites")]
    public bool canMine = false;
    public bool canChop = false;
    public bool canBuild = false;
    public MonsterType monsterType;
    private EnergySystem energySystem;
    private HouseManager houseManager;

    [Header("States")]
    public monsterState currentState;
    public float intervalBetweenStates = 30;//time before changing states
    private int whichState = 0;
    public float currentTime;
    public bool isFollowingCommand = false;

    [Header("UI")]
    public TextMeshProUGUI stateText;
    public Image radialTimer;
    public Material outliner;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        energySystem = GetComponent<EnergySystem>();
        houseManager = FindObjectOfType<HouseManager>();

        wanderBehaviour = GetComponent<WanderBehaviour>();
        miningBehaviour = GetComponent<MiningBehaviour>();
        choppingBehaviour = GetComponent<ChoppingBehaviour>();
        buildingBehaviour = GetComponent<BuildingBehaviour>();

        currentTime = 0f;
    }

    public void WhatType()
    {
        switch (monsterType.role)
        {
            case MonsterRole.Lumber:
                canChop = true;
                break;
            case MonsterRole.Miner:
                canMine = true;
                break;
            case MonsterRole.Builder:
                canBuild = true;
                break;
        }
    }

    private void Update()
    {
        if (energySystem.isLowEnegry && !isFollowingCommand)
        {
            Recharge();
            return;
        }

        if (isFollowingCommand)//only allows the rest of the code to run if it is not already following a command
        {
            return;
        }

        if (currentState == monsterState.mining || currentState == monsterState.chopping || currentState == monsterState.building)
        {
            return;
        }

        currentTime -= Time.deltaTime;//state timer starts counting down
        radialTimer.fillAmount = currentTime / intervalBetweenStates; // Changing timer radial fill amount to current time

        if (currentTime <= 0)//when the timer reaches 0 choose a new state
        {
            whichState = Random.Range(1, 3);//randomly picks a state
            monsterState newState;

            if (whichState == 1)
            {
                newState = monsterState.wandering;//sets the state to wandering
            }
            else
            {
                newState = monsterState.idle;//sets the state to idle
            }

            ChangeState(newState);
        }
    }

    public void ChangeState(monsterState newState)
    {
        agent.isStopped = false;//enables agents movement


        currentState = newState;
        switch (newState)
        {
            case monsterState.idle:
                stateText.text = "Idle";
                stateText.color = Color.red;
                agent.isStopped = true;//stops the agent
                currentTime = intervalBetweenStates / 3;
                break;
            case monsterState.wandering:
                stateText.text = "Wandering";
                stateText.color = Color.blue;
                currentTime = intervalBetweenStates;
                StartCoroutine(wanderBehaviour.Wander());//starts the wander coroutine from the wanderbehaviour script
                break;
            case monsterState.following:
                stateText.text = "Following";
                stateText.color = Color.green;
                break;
            case monsterState.mining:
                if (canMine)
                {
                    stateText.text = "Mining";
                    stateText.color = Color.gray;
                    miningBehaviour.StartMining();//calls the start mining function from the miningbeahviour script
                }
                break;
            case monsterState.chopping:
                if (canChop)
                {
                    stateText.text = "Chopping";
                    stateText.color = Color.yellow;
                    choppingBehaviour.StartChopping();//calls the start chopping function from the choppingbeahviour script
                }
                break;
            case monsterState.building:
                if (canBuild)
                {
                    stateText.text = "Building";
                    stateText.color = Color.magenta;
                    agent.isStopped = true;
                }
                break;
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (buildingBehaviour == null || !buildingBehaviour.isBusy)
        {
            isFollowingCommand = true;
            agent.SetDestination(destination);//moves the agent to where clicked
            ChangeState(monsterState.following);
            StartCoroutine(CheckIfStopped());//checks when the monster has reached its destintaion
        }
    }

    private IEnumerator CheckIfStopped()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)//only alows the code to continue once the monster has reached its destination
        {
            yield return null;
        }

        isFollowingCommand = false;
        if (currentState != monsterState.mining && currentState != monsterState.chopping && currentState != monsterState.building)
        {
            ChangeState(monsterState.wandering);
        }
    }

    public void StartResourceTask(Vector3 resourcePosition, monsterState resourceState)
    {
        if (resourceState == monsterState.mining && !canMine || resourceState == monsterState.chopping && !canChop)//checks that the monster can perform the action
        {
            Debug.Log("Monster cannot perform this action!");
            return;
        }

        isFollowingCommand = true;
        agent.SetDestination(resourcePosition);//sets the agents target to the resource
        ChangeState(monsterState.following);//makes the monsters state following
        StartCoroutine(CheckIfArrived(resourceState));//checks when the monsters arrived
    }

    public bool CanScare()
    {
        return !isFollowingCommand && !energySystem.isLowEnegry && currentState != monsterState.building;
    }

    public void ScareTarget(Enemy_Manager target, float energyCost)
    {
        isFollowingCommand = true;
        energySystem.DrainEnergy(energyCost);
        target.GetComponentInChildren<Renderer>().material = outliner;
        agent.SetDestination(target.transform.position);
        ChangeState(monsterState.following);

        StartCoroutine(ScareRoutine(target));
    }

    private IEnumerator ScareRoutine(Enemy_Manager target)
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        target.Scare();
        isFollowingCommand = false;
        target.GetComponentInChildren<Renderer>().material = null;
        ChangeState(monsterState.wandering);
    }


    private IEnumerator CheckIfArrived(monsterState resourceState)
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)//only alows the code to continue once the monster has reached its destination
        {
            yield return null;
        }

        isFollowingCommand = false;

        //sets the approriate state depending on their ability
        if (resourceState == monsterState.mining && canMine)
        {
            ChangeState(monsterState.mining);
        }
        else if (resourceState == monsterState.chopping && canChop)
        {
            ChangeState(monsterState.chopping);
        }
        else
        {
            ChangeState(monsterState.wandering);
        }
    }

    public void Recharge()
    {
        House house = houseManager.AvailableHouses(monsterType.role);
        isFollowingCommand = true;
        ChangeState(monsterState.idle);
        if (house != null)
        {
            agent.SetDestination(house.transform.position);
            agent.isStopped = false;
            ChangeState(monsterState.following);
            StartCoroutine(EnterHouse(house));
        }
    }

    private IEnumerator EnterHouse(House house)
    { 
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        house.EnterHouse(energySystem);
        energySystem.StartRecharging(house);
        isFollowingCommand = false;
    }
}
