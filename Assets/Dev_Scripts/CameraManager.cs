using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera zoomedOutCamera;
    public float zoomSpeed = 10f;
    public float minZoom = 10f;
    public float maxZoom = 60f;
    public float zoomThreshold = 55f;
    private float targetZoom;

    // Start is called before the first frame update
    void Start()
    {
        targetZoom = mainCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            targetZoom -= scroll * zoomSpeed * 10f;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        if (targetZoom >= zoomThreshold)
        {
            mainCamera.enabled = false;
            zoomedOutCamera.enabled = true;
        }
        else
        {
            mainCamera.enabled = true;
            zoomedOutCamera.enabled = false;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetZoom, Time.deltaTime);
        }
    }
}
