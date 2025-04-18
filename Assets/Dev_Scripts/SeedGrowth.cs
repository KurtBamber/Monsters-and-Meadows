using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGrowth : MonoBehaviour
{
    public Seed seedData;
    public GameObject smallStage;
    public GameObject mediumStage;
    public GameObject finalStage;

    private bool isFullyGrown = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isFullyGrown)//if the seed if fully grown and lmb is pressed harvest
        {
            Harvest();
        }
    }

    public void StartGrowing(Seed seed)
    {
        seedData = seed;
        StartCoroutine(Grow(seed.growthTime));//starts the grow coroutine getting the growth time from the seed
    }

    private IEnumerator Grow(float growthTime)
    {
        GameObject small = Instantiate(smallStage, transform.position, Quaternion.identity);//spawns the small stage of the seed

        yield return new WaitForSeconds(growthTime / 2);//waits for the growth time 

        Destroy(small);//destroys the small stage of the seed
        GameObject medium = Instantiate(mediumStage, transform.position, Quaternion.identity);//spawns the medium stage of the seed

        yield return new WaitForSeconds(growthTime / 2);
        Destroy(medium);//destroys the medium stage of the seed
        finalStage.SetActive(true);//sets the final stage to active so it can check for collisions
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
                Instantiate(seedData.monsterPrefab, transform.position, Quaternion.identity);//spawns the corresponding monster
                Destroy(gameObject);//destroys the seed
            }
        }
    }
}
