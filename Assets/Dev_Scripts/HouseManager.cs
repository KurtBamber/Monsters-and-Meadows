using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    private List<House> houses = new List<House>();

    public void AddHouse(House house)
    {
        if (!houses.Contains(house))
        {
            houses.Add(house);
        }
    }

    public House AvailableHouses(MonsterType.MonsterRole preferredHouse)
    {
        foreach (var house in houses)
        {
            if (house.houseType == preferredHouse && house.hasSpace)
            {
                return house;
            }
        }

        foreach (var house in houses)
        {
            if (house.hasSpace)
            {
                return house;
            }
        }

        return null;
    }
}
