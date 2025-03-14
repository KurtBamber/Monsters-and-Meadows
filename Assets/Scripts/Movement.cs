using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
