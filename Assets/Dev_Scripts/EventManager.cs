using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Event Properties")]
    public int randomNum;
    public int scarinessOfMonsters;
    public float howLongTillNextEvent;
    public string[] eventNotifications;

    [Header("Raid Properties")]
    public int raidDifficulty;

    [Header("Human Scout Properties")]
    public int likelyhoodOfScoutReporting;

    [Header("Monster Encounter Properties")]
    public int likelyhoodOfMonsterEncounter;
    public int whichMonster;
    public GameObject[] monsters;

    [Header("Potential Trader Properties")]
    public int traderLuck;
    public int whichTrader;
    public GameObject[] traders;
}
