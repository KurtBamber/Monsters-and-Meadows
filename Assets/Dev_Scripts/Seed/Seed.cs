using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSeed", menuName = "Seeds")]

public class Seed : ScriptableObject
{
    public string seedName;
    public Sprite seedIcon;
    public GameObject monsterPrefab;
    public float growthTime;
    public GameObject plantPrefab;
    public MonsterType monsterType;
}
