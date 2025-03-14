using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("AI Navigation")]
    public NavMeshAgent agent; // Monster nav agent
    public Transform Monster; // Monsters position

    [Header("States")]
    public int whichState; // Which state is the monster in currently
    public float intervalBetweenStates = 60; // Set time between picking states
    public float currentTime = 60; // Time which gets reduced as state is played

    [Header("Wandering")]
    public float moveRadius = 5f; // Radius around the monster which it can move in
    public float moveInterval = 10f; // Time between position changes

    [Header("UI")]
    public TextMeshProUGUI stateText; // Text showing which state is active
    public TextMeshProUGUI stateTimer; // Timer showing how long state is active



    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>(); // Getting monster agent
        Monster = this.gameObject.transform; // Getting transform

        MonsterWander(); // Setting all monsters to wander on start
    }

    private void Update()
    {

        currentTime -= Time.deltaTime; // State timer starts reducing
        stateTimer.text = currentTime.ToString("F2"); // Text is set to timer with only 2 decimal places

        if (currentTime <= 0) // When current time reaches zero it randomises state
        {
            whichState = Random.Range(1, 3);

            if (whichState == 1)
            {
                MonsterWander(); // If state is one the monster wanders
            }
            if (whichState == 2)
            {
                MonsterIdle(); // If state is two the monster is idle
            }
            
        }
    }

    public void MonsterWander() // Changing text and colour, starting monster wandering and resetting timer
    {
        stateText.text = "Wandering";
        stateText.color = Color.blue;

        StartCoroutine(MoveMonster());
        currentTime = intervalBetweenStates;
    }

    public void MonsterIdle() // Changing text and colour, stopping monster movement and resetting timer deviding it by 3 so idle is quicker
    {
        stateText.text = "Idle";
        stateText.color = Color.red;

        agent.isStopped = true;
        currentTime = intervalBetweenStates / 3;
    }
   
    IEnumerator MoveMonster() // Monster can move, loops random position and timer
    {
        agent.isStopped = false;

        while (true)
        {
            MoveToRandomPosition();
            yield return new WaitForSeconds(moveInterval); // wait this amount of time before repeating
        }
    }

    void MoveToRandomPosition() // If there is no monster it returns
    {
        if (Monster == null) return;

        
        Vector2 randomOffset = Random.insideUnitCircle * moveRadius; // Gets a random point in a radius around the object X by the radius
        Vector3 newPosition = new Vector3(Monster.position.x + randomOffset.x, Monster.position.y, Monster.position.z + randomOffset.y); // New position is the model + random position

        agent.SetDestination(newPosition); // Moves ai in this direction
        
    }
}
