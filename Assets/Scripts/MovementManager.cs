using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public GameObject indicatorPrefab;
    public List<AIController> selectedMonster = new List<AIController>();//list which checks which monsters you have selected
    public int button;//allows us to change whether the left or right mouse button is used for movement
    private Vector3 startPos;//the position of the mouse when beginning to drag
    private float maxDrag = 10f;//the max distance before you start selecting
    public RectTransform selectionBox;
    public Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(button))//on mouse click
        {
            startPos = Input.mousePosition;//gets the start position of the mouse when the mouse is pressed
            selectionBox.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(button))//while the mouse button is held
        {
            UpdateSelectionBox();
        }

        if (Input.GetMouseButtonUp(button))//when mouse button is let go
        {
            float dragDistance = Vector2.Distance(startPos, Input.mousePosition);//calculates the distance dragged

            if (dragDistance > maxDrag)//checks if the drag distance is greater than the max
            {
                SelectMonsters();
            }
            else
            {
                MoveMonsters();
            }
            selectionBox.gameObject.SetActive(false);
        }
    }

    private void SelectMonsters()
    {
        Vector2 screenStartPos = startPos;//start of the drag
        Vector2 screenEndPos = Input.mousePosition;//end of the drag

        Rect selectionRect = new Rect(//creates a rectangle according to the drag
            Mathf.Min(screenStartPos.x, screenEndPos.x),
            Mathf.Min(screenStartPos.y, screenEndPos.y),
            Mathf.Abs(screenStartPos.x - screenEndPos.x),
            Mathf.Abs(screenStartPos.y - screenEndPos.y)
        );

        selectedMonster.Clear();//clears any previously selected monsters

        foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster"))//goes through all the monsters in the scene
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(monster.transform.position);//converts the monsters world pos to screen space
            if (selectionRect.Contains(screenPos))//checks if the monsters are within the selection area
            {
                selectedMonster.Add(monster.GetComponent<AIController>());//adds the monster to the list
            }
        }
    }

    private void UpdateSelectionBox()
    {
        //calculates the width and height of selection box based on mouse movement
        float boxWidth = Input.mousePosition.x - startPos.x;
        float boxHeight = Input.mousePosition.y - startPos.y;

        Vector2 localStartPos = selectionBox.parent.InverseTransformPoint(startPos);//converts mouses start point from screen space to local space
        Vector2 localMousePos = selectionBox.parent.InverseTransformPoint(Input.mousePosition);//converts current pos from screen space to local space

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(localMousePos.x - localStartPos.x), Mathf.Abs(localMousePos.y - localStartPos.y));//sets the size of the dragged box based on drag distance
        selectionBox.anchoredPosition = new Vector2(localStartPos.x + (localMousePos.x - localStartPos.x) / 2, localStartPos.y + (localMousePos.y - localStartPos.y) / 2);//sets the position of the selection box to the center of the dragged area
    }

    private void MoveMonsters()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//sends a ray from the main camera to the point that you clicked
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))//checks if the ray collided with anything
        {
            if (hit.collider.CompareTag("Monster"))//checks if the player clicked on a monster
            {
                selectedMonster.Clear();
                selectedMonster.Add(hit.collider.GetComponent<AIController>());//if so changes the selected monster to that monster
            }
            else if (selectedMonster.Count > 0)//checks that there is a monster selected
            {
                foreach (AIController monster in selectedMonster)//moves each monster
                {
                    monster.Follow(hit.point);
                }

                GameObject indicator = Instantiate(indicatorPrefab, hit.point, indicatorPrefab.transform.rotation);//spawns an indicator where clicked

                    Destroy(indicator, 0.5f);//destroys the indicator after 0.3 seconds
              
            }
        }
    }
}
