using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    [Header("AI Navigation")]
    public NavMeshAgent agent; // Monster nav agent
    public Transform Monster; // Monsters position

    [Header("States")]
    public int whichState; // Which state is the monster in currently
    public string currentState; // State as string for other scripts
    public float intervalBetweenStates = 60; // Set time between picking states
    public float currentTime = 60; // Time which gets reduced as state is played

    [Header("Wandering")]
    public float moveRadius = 5f; // Radius around the monster which it can move in
    public float moveInterval = 10f; // Time between position changes

    [Header("Following")]
    public bool isFollowing;
    private MovementManager MM;
    public GameObject movementManager;

    [Header("UI")]
    public TextMeshProUGUI stateText; // Text showing which state is active
    public Image radialTimer; // Radial image to show timer


    

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>(); // Getting monster agent
        Monster = this.gameObject.transform; // Getting transform
        MM = movementManager.GetComponent<MovementManager>();

        MonsterWander(); // Setting all monsters to wander on start
    }

    private void Update()
    {

        if (agent.velocity.magnitude <= 0.15f && isFollowing)
        {
            MM.selectedMonster.Clear();
            isFollowing = false;
            currentTime = 0.1f;
            Debug.Log("monster stopped");

        }

        if (!isFollowing)
        {
            currentTime -= Time.deltaTime; // State timer starts reducing

            radialTimer.fillAmount = currentTime / intervalBetweenStates;

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
    }
    public void Follow(Vector3 destination)
    {
        isFollowing = true;

        currentState = "Following";

        stateText.text = "Following";
        stateText.color = Color.green;

        whichState = 3;

        agent.SetDestination(destination);

        if (!MM.selectedMonster.Contains(this))
        {
            MonsterWander();
        }

    }

    public void MonsterWander() // Changing text and colour, starting monster wandering and resetting timer
    {
            currentState = "Wandering";
            stateText.text = "Wandering";
            stateText.color = Color.blue;

            StartCoroutine(MoveMonster());
            currentTime = intervalBetweenStates;
    }

    public void MonsterIdle() // Changing text and colour, stopping monster movement and resetting timer deviding it by 3 so idle is quicker
    {
        currentState = "Idle";
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
        if (!isFollowing)
        {
            if (Monster == null) return;

            Vector2 randomOffset = Random.insideUnitCircle * moveRadius; // Gets a random point in a radius around the object X by the radius
            Vector3 newPosition = new Vector3(Monster.position.x + randomOffset.x, Monster.position.y, Monster.position.z + randomOffset.y); // New position is the model + random position

            agent.SetDestination(newPosition); // Moves ai in this direction

        }

    }
}
