using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowManager : MonoBehaviour
{
    [Header("Following Mechanic")]
    [Header("Cameras and Objects")]
    public GameObject mainCam;

    [Header("Zooming Parameters")]
    public Quaternion currentCamRotation;

    [Header("Going To The Garden")]
    public MovementManager MM;
    public CameraManager CameraManager;
    public bool inGarden;
    public float swappingSpeed;

    [Header("Positions and Rotations")]
    public Quaternion villageCamRotation;
    public Quaternion gardenCamRotation;
    public Vector3 gardenCamPosition;

    public void Start()
    {
        inGarden = false;
    }

    public void Update()
    {    
        if (!inGarden && Input.GetKeyDown(KeyCode.Return))
        {
            inGarden=true;
        }
        else if (inGarden && Input.GetKeyDown(KeyCode.Return))
        {
            inGarden = false;
        }

        if (inGarden == true && !CameraManager.isZoomedOut)
        {
            InGarden();
        }

        currentCamRotation = mainCam.transform.rotation;
    }
    public void InGarden()
    {
        mainCam.transform.position = (Vector3.Lerp(mainCam.transform.position, gardenCamPosition, Time.deltaTime * swappingSpeed));
        mainCam.transform.rotation = (Quaternion.Lerp(mainCam.transform.rotation, gardenCamRotation, Time.deltaTime * 2.5f));
    }
}
