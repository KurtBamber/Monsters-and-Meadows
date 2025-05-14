using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildMenu;
    public Button buildersHouseButton;
    public Button minersHouseButton;
    public Button lumberersHouseButton;
    public Button storageButton;
    private Vector3 selectedPlotPosition;
    private Quaternion selectedPlotRotation;
    private GameObject selectedPlot;
    public Buildings buildersHouse;
    public Buildings minersHouse;
    public Buildings lumberersHouse;
    public Buildings storage;
    public GameObject underConstructionPrefab;


    // Start is called before the first frame update
    void Start()
    {
        buildersHouseButton.onClick.AddListener(() => Build(buildersHouse));//on clicking the house button call the build func with the house object
        minersHouseButton.onClick.AddListener(() => Build(minersHouse));
        lumberersHouseButton.onClick.AddListener(() => Build(lumberersHouse));
        storageButton.onClick.AddListener(() => Build(storage));//on clicking the storage button call the build func with the storage object
    }

    public void OpenBuildMenu(Vector3 position, Quaternion rotation, GameObject plot)
    {
        selectedPlotPosition = position;
        selectedPlotRotation = rotation;
        selectedPlot = plot;
        buildMenu.SetActive(true);//enables the building menu
    }

    public void CloseBuildMenu()
    {
        selectedPlot = null;
        buildMenu.SetActive(false);
    }

    public void Build(Buildings choice)
    {
        if (!GetAvailableBuilder())
        {
            return;
        }

        if (ResourceManager.resourceManager.totalWood >= choice.requiredWood && ResourceManager.resourceManager.totalStone >= choice.requiredStone)//checks the resource manager against the scriptable object to see if they have enough materials
        {
            ResourceManager.resourceManager.totalWood -= choice.requiredWood;//deducts the used resource
            ResourceManager.resourceManager.totalStone -= choice.requiredStone;
            ResourceManager.resourceManager.UpdateUI();

            GameObject underConstruction = Instantiate(underConstructionPrefab, selectedPlotPosition, selectedPlotRotation);//spawns the chosen building at the plots location and rotation
            AssignBuilder(choice, underConstruction);
            Destroy(selectedPlot);
            CloseBuildMenu();
        }
    }

    public bool GetAvailableBuilder()
    {
        MonsterAI[] monsters = FindObjectsOfType<MonsterAI>();
        foreach (MonsterAI monster in monsters)
        {
            if (monster.canBuild && (monster.currentState == monsterState.idle || monster.currentState == monsterState.wandering))
            {
                return true;
            }
        }
        return false;
    }

    private void AssignBuilder(Buildings building, GameObject underConstruction)
    {
        MonsterAI[] monsters = FindObjectsOfType<MonsterAI>();
        foreach (MonsterAI monster in monsters)
        {
            if (monster.canBuild &&
                (monster.currentState == monsterState.idle || monster.currentState == monsterState.wandering))
            {
                BuildingBehaviour builder = monster.GetComponent<BuildingBehaviour>();
                if (builder != null)
                {
                    builder.StartBuilding(underConstruction, building.buildingPrefab);
                    break;
                }
            }
        }
    }
}
