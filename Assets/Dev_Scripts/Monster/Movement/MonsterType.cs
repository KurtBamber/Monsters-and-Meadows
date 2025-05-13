using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterType", menuName = "Monsters")] 
public class MonsterType : ScriptableObject
{
    public enum MonsterRole { Lumber, Miner, Builder };
    public MonsterRole role;
    public float speed;
}
