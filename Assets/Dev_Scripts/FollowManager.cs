using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowManager : MonoBehaviour
{
    [Header("Following Mechanic")]
    [Header("Cameras and Objects")]
    public GameObject mainCam;
    public GameObject devCam;
    public GameObject objectToFollow;
    public GameObject currentCam;
    public GameObject Player;

    [Header("Zooming Parameters")]
    public bool canZoom;
    public float scrollAmount;
    public float zoomedInView;
    public float zoomedOutView;
    public Quaternion currentCamRotation;

    [Header("Going To The Garden")]
    public MovementManager MM;
    public bool inGarden;
    public float swappingSpeed;

    [Header("Positions and Rotations")]
    public Quaternion villageCamRotation;
    public Quaternion gardenCamRotation;
    public Vector3 villageCamPosition;
    public Vector3 gardenCamPosition;

    public void Start()
    {
        InVillage();
        inGarden = false;
    }

    public void Update()
    {
            if (devCam.activeSelf == true)
            {
                currentCam = devCam.gameObject;
            }
            else if (devCam.activeSelf == false)
            {
                currentCam = mainCam.gameObject;
            }
        
        if (!inGarden && Input.GetKeyDown(KeyCode.Return))
        {
            inGarden=true;
        }
        else if (inGarden && Input.GetKeyDown(KeyCode.Return))
        {
            inGarden = false;
        }

        if (inGarden == true)
        {
            InGarden();
        }
        else
        {
            InVillage();
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

        if(Input.mouseScrollDelta.y > 0 && scrollAmount < 1 && canZoom)
        {
            scrollAmount += 1;
        }

        if (Input.mouseScrollDelta.y < 0 && scrollAmount > -0 && canZoom)
        {
            scrollAmount -= 1;
        }
    }
    public void InGarden()
    {
        canZoom = false;
        currentCam.transform.position = (Vector3.Lerp(currentCam.transform.position, gardenCamPosition, Time.deltaTime * swappingSpeed));
        currentCam.transform.rotation = (Quaternion.Lerp(currentCam.transform.rotation, gardenCamRotation, Time.deltaTime * 2.5f));
    }

    public void InVillage()
    {
        canZoom = true;
        currentCam.transform.position = (Vector3.Lerp(currentCam.transform.position, villageCamPosition, Time.deltaTime * swappingSpeed));
        currentCam.transform.rotation = (Quaternion.Lerp(currentCam.transform.rotation, villageCamRotation, Time.deltaTime * 2.5f));
    }

    public void FollowObject()
    {
        if(scrollAmount >= 1 && canZoom)
        {
            currentCam.transform.LookAt(Vector3.Lerp(currentCam.transform.position, objectToFollow.transform.position, Time.deltaTime * 2.5f));
            currentCam.transform.LookAt(objectToFollow.transform.position);
            currentCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCam.GetComponent<Camera>().fieldOfView, zoomedInView, Time.deltaTime * 2.5f);
        }

        if (scrollAmount <= 0 && canZoom)
        {
            currentCam.transform.rotation = Quaternion.Lerp(currentCamRotation, villageCamRotation, Time.deltaTime * 2.5f);
            currentCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCam.GetComponent<Camera>().fieldOfView, zoomedOutView, Time.deltaTime * 2.5f);
        }
    }
}
