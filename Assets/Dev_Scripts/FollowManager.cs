using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowManager : MonoBehaviour
{
    public GameObject mainCam, devCam, objectToFollow, currentCam, Player;

    public MovementManager MM;

    public float scrollAmount, zoomedInView, zoomedOutView;

    public Quaternion defaultCamRotation, currentCamRotation;

    public void Update()
    {
        if(devCam.activeSelf == true)
        {
            currentCam = devCam.gameObject;
        }
        else if(devCam.activeSelf == false)
        {
            currentCam = mainCam.gameObject;
        }

        currentCamRotation = currentCam.transform.rotation;

        if (MM.selectedMonster.Count >= 1)
        {
            objectToFollow = MM.selectedMonster[0].gameObject;
            FollowObject();
        }

        if(MM.selectedMonster.Count <= 0)
        {
            objectToFollow = Player;
            FollowObject();
        }

        if(Input.mouseScrollDelta.y > 0 && scrollAmount < 1)
        {
            scrollAmount += 1;
        }

        if (Input.mouseScrollDelta.y < 0 && scrollAmount > -0)
        {
            scrollAmount -= 1;
        }
    }

    public void FollowObject()
    {
        if(scrollAmount >= 1)
        {
            currentCam.transform.LookAt(Vector3.Lerp(currentCam.transform.position, objectToFollow.transform.position, Time.deltaTime * 2.5f));
            currentCam.transform.LookAt(objectToFollow.transform.position);
            currentCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCam.GetComponent<Camera>().fieldOfView, zoomedInView, Time.deltaTime * 2.5f);
        }

        if (scrollAmount <= 0)
        {
            currentCam.transform.rotation = Quaternion.Lerp(currentCamRotation, defaultCamRotation, Time.deltaTime * 2.5f);
            currentCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCam.GetComponent<Camera>().fieldOfView, zoomedOutView, Time.deltaTime * 2.5f);
        }
    }
}
