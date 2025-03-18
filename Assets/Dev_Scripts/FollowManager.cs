using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowManager : MonoBehaviour
{
    public GameObject mainCam, devCam, objectToFollow;

    public MovementManager MM;

    public float scrollAmount, zoomedInView, zoomedOutView;

    public Quaternion defaultCamRotation, currentCamRotation;

    public void Update()
    {
        currentCamRotation = mainCam.transform.rotation;
        if (MM.selectedMonster.Count >= 1)
        {
            objectToFollow = MM.selectedMonster[0].gameObject;
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
            mainCam.transform.LookAt(Vector3.Lerp(mainCam.transform.position, objectToFollow.transform.position, Time.deltaTime * 2.5f));
            mainCam.transform.LookAt(objectToFollow.transform.position);
            mainCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCam.GetComponent<Camera>().fieldOfView, zoomedInView, Time.deltaTime * 2.5f);
        }

        if (scrollAmount <= 0)
        {
            mainCam.transform.rotation = Quaternion.Lerp(currentCamRotation, defaultCamRotation, Time.deltaTime * 2.5f);
            mainCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCam.GetComponent<Camera>().fieldOfView, zoomedOutView, Time.deltaTime * 2.5f);
        }
    }
}
