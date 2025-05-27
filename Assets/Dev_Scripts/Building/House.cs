using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public MonsterType.MonsterRole houseType;
    private List<EnergySystem> currentOccupants = new List<EnergySystem>();
    public int maxOccupants = 2;
    public bool hasSpace = true;

    private void Start()
    {
        hasSpace = true;
    }

    private void OnEnable()
    {
        FindObjectOfType<HouseManager>().AddHouse(this);
    }

    private void UpdateHasSpace()
    {
        hasSpace = currentOccupants.Count < maxOccupants;
    }

    public void EnterHouse(EnergySystem monster)
    {
        if (!currentOccupants.Contains(monster) && hasSpace)
        {
            currentOccupants.Add(monster);
            UpdateHasSpace();
        }
    }

    public void LeaveHouse(EnergySystem monster)
    {
        if (currentOccupants.Contains(monster))
        {
            currentOccupants.Remove(monster);
            UpdateHasSpace();
        }
    }
}
