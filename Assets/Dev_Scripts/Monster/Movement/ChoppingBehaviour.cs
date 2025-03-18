using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBehaviour : MonoBehaviour
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

    public void StartChopping()
    {
        StartCoroutine(ChopResource());
    }

    private IEnumerator ChopResource()
    {
        while (monsterAI.currentState == monsterState.chopping)
        {
            ResourceManager.resourceManager.AddWood(amount);
            yield return new WaitForSeconds(interval);
        }
    }
}
