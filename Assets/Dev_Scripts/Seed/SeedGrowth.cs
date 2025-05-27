using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SeedGrowth : MonoBehaviour
{
    public Seed seedData;
    public bool isFullyGrown = false;
    private bool firstHarvest = true;
    public Dialogue dialogue;
    private DialogueManager dialogueManager;
    private GameObject dirtObject;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isFullyGrown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    Harvest();
                }
            }
        }

        if (isFullyGrown && dialogueManager.firstHarvest)
        {
            dialogueManager.firstHarvest = false;
            dialogueManager.StartDialogue(dialogue);
        }
    }

    public void StartGrowing(Seed seed, GameObject dirt)
    {
        seedData = seed;
        dirtObject = dirt;
        StartCoroutine(Grow(seed.growthTime));//starts the grow coroutine getting the growth time from the seed
    }

    private IEnumerator Grow(float growthTime)
    {
        transform.localScale = Vector3.zero;
        Vector3 startPos = transform.position;

        float timer = 0f;

        while (timer < growthTime)
        {
            timer += Time.deltaTime;
            float t = timer / growthTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(2, 2, 2), t);
            transform.position = Vector3.Lerp(startPos, startPos + new Vector3(0, 1, 0), t);
            yield return null;
        }

        transform.localScale = new Vector3(2, 2, 2);
        isFullyGrown = true;
    }

    private void Harvest()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//gets the point where the player clicked
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("FullyGrown"))//checks if the clicked object has the fully grown tag
            {
                GameObject monster = Instantiate(seedData.monsterPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), Quaternion.identity);//spawns the corresponding monster
                MonsterAI monsterAI = monster.GetComponent<MonsterAI>();
                monsterAI.monsterType = seedData.monsterType;
                monsterAI.WhatType();

                if (firstHarvest)
                {
                    firstHarvest = false;
                    dialogueManager.waitingForHarvest = false;
                    dialogueManager.DisplayNextSentence();
                }

                Collider[] nearby = Physics.OverlapSphere(transform.position, 1f);
                foreach (Collider col in nearby)
                {
                    Plot plot = col.GetComponent<Plot>();
                    if (plot != null)
                    {
                        plot.isOccupied = false;
                    }
                }

                Destroy(gameObject);//destroys the seed
                Destroy(dirtObject);
            }
        }
    }
}