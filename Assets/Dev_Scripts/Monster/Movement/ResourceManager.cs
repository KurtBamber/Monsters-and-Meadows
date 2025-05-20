using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager resourceManager;
    public int totalStone = 0;
    public int totalWood = 0;
    public TextMeshProUGUI woodAmount, stoneAmount;
    public GameObject[] woodLogs, Stones;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = this;
        UpdateUI();
    }

    private void Update()
    {
        /*
        for (int i = 0; i < totalWood; i++)
        {
            woodLogs[i].SetActive(true);
        }

        for (int i = 0; i < totalStone; i++)
        {
            Stones[i].SetActive(true);
        }
        */
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

    public void RemoveStone(int amount)
    {
        if (totalStone >= amount)
        {
            totalStone -= amount;
            UpdateUI();
        }
    }

    public void RemoveWood(int amount)
    {
        if (totalWood >= amount)
        {
            totalWood -= amount;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        woodAmount.text = totalWood.ToString();
        stoneAmount.text = totalStone.ToString();

       
    }
}
