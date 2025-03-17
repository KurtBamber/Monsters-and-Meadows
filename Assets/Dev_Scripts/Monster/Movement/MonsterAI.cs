using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum monsterState { idle, wandering, following, mining, chopping }

public class MonsterAI : MonoBehaviour
{
    [Header("AI Navigation")]
    private NavMeshAgent agent;
    private WanderBehaviour wanderBehaviour;
    private MiningBehaviour miningBehaviour;
    private ChoppingBehaviour choppingBehaviour;
    private GameObject targetResource;

    [Header("Abilites")]
    public bool canMine = false;
    public bool canChop = false;

    [Header("States")]
    public monsterState currentState;
    public float intervalBetweenStates = 30;//time before changing states
    private int whichState = 0;
    public float currentTime;
    private bool isFollowingCommand = false;

    [Header("UI")]
    public TextMeshProUGUI stateText;
    public Image radialTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        wanderBehaviour = GetComponent<WanderBehaviour>();
        miningBehaviour = GetComponent<MiningBehaviour>();
        choppingBehaviour = GetComponent<ChoppingBehaviour>();

        currentTime = 0f;
    }

    private void Update()
    {
        if (isFollowingCommand)//only allows the rest of the code to run if it is not already following a command
        {
            return;
        }

        currentTime -= Time.deltaTime;//state timer starts counting down
        radialTimer.fillAmount = currentTime / intervalBetweenStates;//timer is set to show only two decimal points

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
                StartCoroutine(wanderBehaviour.Wander());//starts the wander coroutine form the wanderbehaviour script
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
                    miningBehaviour.StartMining(targetResource);//calls the start mining function from the miningbeahviour script
                }
                break;
            case monsterState.chopping:
                if (canChop)
                {
                    stateText.text = "Chopping";
                    stateText.color = Color.yellow;
                    choppingBehaviour.StartChopping(targetResource);//calls the start chopping function from the choppingbeahviour script
                }
                break;
        }
    }

    public void MoveTo(Vector3 destination)
    {
        isFollowingCommand = true;
        agent.SetDestination(destination);//moves the agent to where clicked
        ChangeState(monsterState.following);
        StartCoroutine(CheckIfStopped());//checks when the monster has reached its destintaion
    }

    private IEnumerator CheckIfStopped()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)//only alows the code to continue once the monster has reached its destination
        {
            yield return null;
        }

        isFollowingCommand = false;
        ChangeState(monsterState.wandering);//sets monster back to wandering
    }

    public void StartResourceTask(Vector3 resourcePosition, GameObject resource, monsterState resourceState)
    {
        if (resourceState == monsterState.mining && !canMine || resourceState == monsterState.chopping && !canChop)//checks that the monster can perform the action
        {
            Debug.Log("Monster cannot perform this action!");
            return;
        }

        isFollowingCommand = true;
        targetResource = resource;
        agent.SetDestination(resourcePosition);//sets the agents target to the resource
        ChangeState(monsterState.following);//makes the monsters state following
        StartCoroutine(CheckIfArrived(resourceState));//checks when the monsters arrived
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
}
