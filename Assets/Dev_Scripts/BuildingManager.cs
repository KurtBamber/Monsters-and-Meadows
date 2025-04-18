using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildMenu;
    public Button houseButton;
    public Button storageButton;
    private Vector3 selectedPlotPosition;
    private Quaternion selectedPlotRotation;
    private GameObject selectedPlot;
    public Buildings house;
    public Buildings storage;

    // Start is called before the first frame update
    void Start()
    {
        houseButton.onClick.AddListener(() => Build(house));//on clicking the house button call the build func with the house object
        storageButton.onClick.AddListener(() => Build(storage));//on clicking the storage button call the build func with the storage object
    }

    public void OpenBuildMenu(Vector3 position, Quaternion rotation, GameObject plot)
    {
        selectedPlotPosition = position;
        selectedPlotRotation = rotation;
        selectedPlot = plot;
        buildMenu.SetActive(true);//enables the building menu
    }

    public void Build(Buildings choice)
    {
        if (ResourceManager.resourceManager.totalWood >= choice.requiredWood && ResourceManager.resourceManager.totalStone >= choice.requiredStone)//checks the resource manager against the scriptable object to see if they have enough materials
        {
            ResourceManager.resourceManager.totalWood -= choice.requiredWood;//deducts the used resource
            ResourceManager.resourceManager.totalStone -= choice.requiredStone;
            ResourceManager.resourceManager.UpdateUI();

            Instantiate(choice.buildingPrefab, selectedPlotPosition, selectedPlotRotation);//spawns the chosen building at the plots location and rotation
            buildMenu.SetActive(false);//disables the building menu
        }
    }
}
