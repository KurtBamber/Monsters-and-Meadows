using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager resourceManager;
    public int totalStone = 0;
    public int totalWood = 0;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = this;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void AddStone(int amount)
    {
        totalStone += amount;
        UpdateUI();
    }

    public void AddWood(int amount)
    {
        totalWood += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {

    }
}
