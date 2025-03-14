using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("AI Navigation")]
    public NavMeshAgent agent;
    public Transform Monster;

    [Header("Wandering")]
    public float moveRadius = 5f; // Radius around the monster which it can move in
    public float moveInterval = 10f; // Time between position changes



    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(MoveMonster());
    }


    IEnumerator MoveMonster()
    {
        while (true)
        {
            MoveToRandomPosition();
            yield return new WaitForSeconds(moveInterval);
        }
    }

    void MoveToRandomPosition()
    {
        if (Monster == null) return;

        // Get a random position within the radius of the monster
        Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
        Vector3 newPosition = new Vector3(Monster.position.x + randomOffset.x, Monster.position.y, Monster.position.z + randomOffset.y);

        agent.SetDestination(newPosition);
        
    }
}
