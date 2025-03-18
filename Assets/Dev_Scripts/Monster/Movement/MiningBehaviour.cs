using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningBehaviour : MonoBehaviour
{
    private MonsterAI monsterAI;
    public int amount = 5;
    public float interval = 5f;

    // Start is called before the first frame update
    void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMining()
    {
        StartCoroutine(MineResource());
    }

    private IEnumerator MineResource()
    {
        while (monsterAI.currentState == monsterState.mining)
        {
            ResourceManager.resourceManager.AddStone(amount);
            yield return new WaitForSeconds(interval);
        }
    }
}
