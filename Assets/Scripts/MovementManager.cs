using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public GameObject indicatorPrefab;
    private Movement selectedMonster;//checks which monster you have selected
    public int button;//allows us to change whether the left or right mouse button is used for movement

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(button))//on mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//sends a ray from the main camera to the point that you clicked
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))//checks if the ray collided with anything
            {
                if (hit.collider.CompareTag("Monster"))//checks if the player clicked on a monster
                {
                    selectedMonster = hit.collider.GetComponent<Movement>();//if so changes the selected monster to that monster
                }
                else if (selectedMonster != null)//checks that there is a monster selected
                {
                    selectedMonster.MoveTo(hit.point);//calls the MoveTo function from the monsters movement script
                    GameObject indicator = Instantiate(indicatorPrefab, hit.point, indicatorPrefab.transform.rotation);//spawns an indicator where clicked
                    Destroy(indicator, 0.3f);//destroys the indicator after 0.3 seconds
                }
            }
        }
    }
}
