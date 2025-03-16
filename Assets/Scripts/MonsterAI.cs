using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum monsterState { idle, wandering, following }

public class MonsterAI : MonoBehaviour
{
    [Header("AI Navigation")]
    private NavMeshAgent agent;
    private WanderBehaviour wanderBehaviour;

    [Header("States")]
    public monsterState currentState;
    public float intervalBetweenStates = 30;//time before changing states
    private int whichState = 0;
    private float currentTime;
    private bool isFollowingCommand = false;

    [Header("UI")]
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI stateTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderBehaviour = GetComponent<WanderBehaviour>();

        currentTime = 0f;
    }

    private void Update()
    {
        if (isFollowingCommand)//only allows the rest of the code to run if it is not already following a command
        {
            return;
        }

        currentTime -= Time.deltaTime;//state timer starts counting down
        stateTimer.text = currentTime.ToString("F2");//timer is set to show only two decimal points

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
}
