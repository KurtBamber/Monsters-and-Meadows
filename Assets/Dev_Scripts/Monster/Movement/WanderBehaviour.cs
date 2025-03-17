using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class WanderBehaviour : MonoBehaviour
{
    [Header("Wandering Settings")]
    public float wanderRadius = 50f;
    public float moveInterval = 4f;

    private NavMeshAgent agent;
    private MonsterAI monsterAI;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterAI = GetComponent<MonsterAI>();
    }

    public IEnumerator Wander()
    {
        while (monsterAI.currentState == monsterState.wandering)//checks if the monsters current state is wandering
        {
            MoveToRandomPosition();
            yield return new WaitForSeconds(moveInterval);//time between moving to a new location
        }
    }

    private void MoveToRandomPosition()
    {
        if (monsterAI.currentState != monsterState.wandering)//checks if the monsters current state is wandering
        {
            return;
        }

        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;//chooses a random point from within the wander radius 
        Vector3 newPosition = new Vector3(transform.position.x + randomOffset.x, transform.position.y, transform.position.z + randomOffset.y);//creates a new position based on the random point 

        agent.SetDestination(newPosition);//moves towards the random position

    }
}
