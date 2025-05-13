using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectorManager : MonoBehaviour
{
    public GameObject inspectorUI, selectedMonster;
    public bool isInspectorOpen;
    public MovementManager MM;

    public TextMeshProUGUI monsterTrait, monsterActivity;

    public float monsterTime;

    public Image radialTimer;
    public Material outlineMaterial;
    public Button nextButton;
    private int currentMonster = 0;


    public void Start()
    {
        inspectorUI = this.gameObject.transform.GetChild(0).gameObject;
        inspectorUI.SetActive(false);

        isInspectorOpen = false;
        nextButton.onClick.AddListener(ShowNextMonster);
    }

    public void Update()
    {
        if(MM.selectedMonster.Count > 0)
        {
            UpdateMonsterActivity();
        }
        else
        {
            ClosePanel();
        }

        if (Input.GetKeyDown(KeyCode.I) && !isInspectorOpen)
        {
            OpenPanel();
        }
        else if(Input.GetKeyDown(KeyCode.I) && isInspectorOpen)
        {
            ClosePanel();
        }
    }

    public void UpdateMonsterActivity()
    {
        selectedMonster = MM.selectedMonster[currentMonster].gameObject;
        monsterActivity.text = MM.selectedMonster[currentMonster].GetComponent<MonsterAI>().stateText.text;
        foreach (var monsters in MM.selectedMonster)
        {
            monsters.GetComponentInChildren<Renderer>().material = outlineMaterial;
        }

        if (MM.selectedMonster[currentMonster].GetComponent<MonsterAI>().canChop)
        {
            monsterTrait.text = "Lumberer";
        }
        else if (MM.selectedMonster[currentMonster].GetComponent<MonsterAI>().canMine)
        {
            monsterTrait.text = "Miner";
        }
        else if (MM.selectedMonster[currentMonster].GetComponent<MonsterAI>().canBuild)
        {
            monsterTrait.text = "Builder";
        }


        monsterTime = MM.selectedMonster[currentMonster].GetComponent<MonsterAI>().currentTime;
        radialTimer.fillAmount = monsterTime / MM.selectedMonster[currentMonster].intervalBetweenStates;
        FindObjectOfType<CameraManager>().selectedMonster = MM.selectedMonster[currentMonster].transform;
    }

    public void ShowNextMonster()
    {
        Debug.Log("Next button clicked!");
        currentMonster++;
        if (currentMonster >= MM.selectedMonster.Count)
        {
            currentMonster = 0;
        }
        UpdateMonsterActivity();
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
