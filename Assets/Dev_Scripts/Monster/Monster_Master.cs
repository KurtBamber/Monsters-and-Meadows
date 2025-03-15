using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Monster_Master : MonoBehaviour
{
    [Header("Monster Info")]
    public string monsterTrait;

    private void Start()
    {
        LowerCase();
    }

    public void LowerCase()
    {
        monsterTrait = monsterTrait.ToLower();
    }
}
