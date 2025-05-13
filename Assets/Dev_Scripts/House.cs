using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public MonsterType.MonsterRole houseType;
    private List<EnergySystem> currentOccupants = new List<EnergySystem>();
    public int maxOccupants = 2;
    public bool hasSpace;

    // Start is called before the first frame update
    private void OnEnabled()
    {
        FindObjectOfType<HouseManager>().AddHouse(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentOccupants.Count >= maxOccupants)
        {
            hasSpace = false;
        }
        else
        {
            hasSpace = true;
        }
    }

    public void EnterHouse(EnergySystem monster)
    {
        if (!currentOccupants.Contains(monster) && hasSpace)
        {
            currentOccupants.Add(monster);
        }
    }

    public void LeaveHouse(EnergySystem monster)
    {
        if (currentOccupants.Contains(monster))
        {
            currentOccupants.Remove(monster);
        }
    }
}
