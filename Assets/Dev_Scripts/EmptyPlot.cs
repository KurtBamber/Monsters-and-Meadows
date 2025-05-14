using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmptyPlot : MonoBehaviour
{
    private bool inRange = false;
    BuildingManager buildingManager;
    public LayerMask plotLayer;

    private void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inRange)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, plotLayer))
            {
                if (hit.collider.CompareTag("EmptyPlot") && hit.collider.gameObject == gameObject)
                {
                    if (buildingManager.buildMenu.activeSelf)
                    {
                        buildingManager.CloseBuildMenu();
                    }
                    else
                    {
                        Transform plot = hit.collider.transform;//gets the transform of the clicked plot
                        buildingManager.OpenBuildMenu(plot.position + new Vector3(0, 1, 0), plot.rotation, plot.gameObject);//sends the plots position to the building manager
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            buildingManager.CloseBuildMenu();
        }
    }
}
