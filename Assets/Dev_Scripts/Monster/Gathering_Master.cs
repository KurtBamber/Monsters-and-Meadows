using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gathering_Master : MonoBehaviour
{
    [Header("Monster")]
    public List<GameObject> triggeredMonsters;
    public GameObject recentlyTriggeredMonster;
    public string whichMonster;

    [Header("Gathering")]
    public int howManyMonsters;
    public int howMuchToGather = 5;
    public int gatheredResource = 5;
    public float gatheringInterval = 5;
    public float currentTime = 5;

    [Header("UI")]
    public TextMeshProUGUI howManyMonstersUI;
    public TextMeshProUGUI howMuchGathered;
    public Image timerBox;


    private void Update()
    {
        howManyMonstersUI.text = howManyMonsters.ToString();
        howMuchGathered.text = gatheredResource.ToString();
        timerBox.rectTransform.sizeDelta = new Vector2(currentTime / gatheringInterval * 100, 10);

        if(howManyMonsters >= 1)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = gatheringInterval;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            
            recentlyTriggeredMonster = other.gameObject;

            if(recentlyTriggeredMonster.GetComponent<Monster_Master>().monsterTrait == whichMonster)
            {
                triggeredMonsters.Add(recentlyTriggeredMonster);
                recentlyTriggeredMonster.GetComponent<CapsuleCollider>().enabled = false;
                howManyMonsters++;

                StartCoroutine(GatherResource());
            }


        }
    }

    IEnumerator GatherResource()
    {
        while (howManyMonsters >= 1)
        {
            gatheredResource += howMuchToGather * howManyMonsters;

            yield return new WaitForSeconds(gatheringInterval);
        }
    }


}
