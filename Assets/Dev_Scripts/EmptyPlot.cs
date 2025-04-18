using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPlot : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("EmptyPlot"))
                {
                    Transform plot = hit.collider.transform;//gets the transform of the clicked plot
                    FindObjectOfType<BuildingManager>().OpenBuildMenu(plot.position + new Vector3(0, 1, 0), plot.rotation, plot.gameObject);//sends the plots position to the building manager
                }
            }
        }
    }
}
