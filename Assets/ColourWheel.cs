using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourWheel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Material playerMat;
    public Color chosenColour;
    public static bool picked;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (chosenColour != eventData.pointerClick.gameObject.GetComponent<Image>().color && !picked)
        {
            chosenColour = eventData.pointerClick.gameObject.GetComponent<Image>().color;
            eventData.pointerClick.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 150);
            picked = true;
        }

        else if(chosenColour == eventData.pointerClick.gameObject.GetComponent<Image>().color && picked)
        {
            chosenColour = Color.black;
            eventData.pointerClick.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            picked = false;
        }

            


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!picked)
        {
            eventData.pointerEnter.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 150);
            playerMat.color = this.gameObject.GetComponent<Image>().color;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!picked)
        {
            eventData.pointerEnter.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        }
        
    }
}
