using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("AI Navigation")]
    public NavMeshAgent agent;
    public Transform Monster;

    [Header("States")]
    public int whichState;
    public float intervalBetweenStates = 60;
    public float currentTime = 60;

    [Header("Wandering")]
    public float moveRadius = 5f; // Radius around the monster which it can move in
    public float moveInterval = 10f; // Time between position changes

    [Header("UI")]
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI stateTimer;



    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        Monster = this.gameObject.transform;

        MonsterWander();
    }

    private void Update()
    {

        currentTime -= Time.deltaTime;
        stateTimer.text = currentTime.ToString("F2");

        if (currentTime <= 0)
        {
            whichState = Random.Range(1, 3);

            if (whichState == 1)
            {
                MonsterWander();
            }
            if (whichState == 2)
            {
                MonsterIdle();
            }
            
        }
    }

    public void MonsterWander()
    {
        stateText.text = "Wandering";
        stateText.color = Color.blue;

        StartCoroutine(MoveMonster());
        currentTime = intervalBetweenStates;
    }

    public void MonsterIdle()
    {
        stateText.text = "Idle";
        stateText.color = Color.red;
        agent.isStopped = true;
        currentTime = intervalBetweenStates / 3;
    }
   
    IEnumerator MoveMonster()
    {
        agent.isStopped = false;
        while (true)
        {
            MoveToRandomPosition();
            yield return new WaitForSeconds(moveInterval); // wait this amount of time before repeating
        }
    }

    void MoveToRandomPosition()
    {
        if (Monster == null) return;

        
        Vector2 randomOffset = Random.insideUnitCircle * moveRadius; // gets a random point in a radius around the object X by the radius
        Vector3 newPosition = new Vector3(Monster.position.x + randomOffset.x, Monster.position.y, Monster.position.z + randomOffset.y); // new position is the model + random position

        agent.SetDestination(newPosition); // moves ai in this direction
        
    }
}
