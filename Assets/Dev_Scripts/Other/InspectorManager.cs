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
    public int currentMonster = 0;
    public Canvas hotbar;
    public Image energyBar;
    private float nextClickCooldown = 0.2f;
    private float lastClickTime = -1f;

    public void Start()
    {
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(ShowNextMonster);
        inspectorUI = this.gameObject.transform.GetChild(0).gameObject;
        inspectorUI.SetActive(false);

        isInspectorOpen = false;
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
        EnergySystem energy = selectedMonster.GetComponent<EnergySystem>();
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
        energyBar.fillAmount = Mathf.Clamp01(energy.currentEnergy / energy.maxEnergy);
        FindObjectOfType<CameraManager>().selectedMonster = MM.selectedMonster[currentMonster].transform;
    }

    public void ShowNextMonster()
    {
        if (Time.time - lastClickTime < nextClickCooldown)
            return;

        lastClickTime = Time.time;
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
        hotbar.enabled = false;
    }

    public void ClosePanel()
    {
        if (isInspectorOpen)
        {
            inspectorUI.SetActive(false);
            isInspectorOpen = false;
            hotbar.enabled = true;
        }
    }
}
