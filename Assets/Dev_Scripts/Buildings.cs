using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Buildings : ScriptableObject
{
    public GameObject buildingPrefab;
    public int requiredWood;
    public int requiredStone;
}
