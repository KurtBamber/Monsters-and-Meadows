using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectorManager : MonoBehaviour
{
    public GameObject inspectorUI, selectedMonster;
    public bool isInspectorOpen;
    public MonsterAI MAI;
    public MovementManager MM;

    public TextMeshProUGUI monsterName, monsterTrait, monsterActivity;

    public float monsterTime;

    public Image radialTimer;


    public void Start()
    {
        inspectorUI = this.gameObject.transform.GetChild(0).gameObject;
        inspectorUI.SetActive(false);

        isInspectorOpen = false;

    }

    public void Update()
    {
        if(MM.selectedMonster.Count > 0)
        {
            OpenPanel();
            UpdateMonsterActivity();
        }
        else
        {
            ClosePanel();
        }

    }

    public void UpdateMonsterActivity()
    {
        selectedMonster = MM.selectedMonster[0].gameObject;

        if (MM.selectedMonster[0].GetComponent<MonsterAI>().canChop)
        {
            monsterActivity.text = "Wood";
        }

        if (MM.selectedMonster[0].GetComponent<MonsterAI>().canMine)
        {
            monsterActivity.text = "Stone";
        }


        monsterTime = MM.selectedMonster[0].GetComponent<MonsterAI>().currentTime;
        radialTimer.fillAmount = monsterTime / MM.selectedMonster[0].intervalBetweenStates;

        monsterName.text = selectedMonster.name;
        monsterTrait.text = selectedMonster.GetComponent<Monster_Master>().monsterTrait + " gatherer";
        
    }


    public void OpenPanel()
    {
        isInspectorOpen = true;
        inspectorUI.SetActive(true);
    }

    public void ClosePanel()
    {
        if (isInspectorOpen)
        {
            inspectorUI.SetActive(false);
            isInspectorOpen = false;
        }
    }
}
