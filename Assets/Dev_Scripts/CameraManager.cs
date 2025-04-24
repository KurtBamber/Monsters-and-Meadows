using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 followOffset;//the offset that the camera follows the player
    public Vector3 zoomedOutPosition;//the cam pos when its fully zoomed out
    public Quaternion zoomedOutRotation;//the cam rotation when its fully zoomed out

    [Header("Zoom Settings")]
    public float minFOV;//the most you can zoom in
    public float maxFOV;//the most you can zoom out
    public float zoomSpeed;

    [Header("Smoothness")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 3f;
    public float fovSmoothSpeed = 5f;//the speed that the fov changes when switching between zoomed in and zoomed out view

    private Camera cam;
    private float targetFOV;
    private bool isZoomedOut = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetFOV = cam.fieldOfView;
    }

    void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");//gets the scroll wheel input as a float value
        targetFOV -= scroll * zoomSpeed * 10f;//each scroll registers as 0.1 or -0.1 
        targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);//ensures that the fov cant go above/below the min/max
        
        isZoomedOut = targetFOV >= maxFOV;//if the fov reaches the max it goes to the zoomed out view

        if (isZoomedOut)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 22f, Time.deltaTime * fovSmoothSpeed);//smoothly transitions the cameras fov to 22 if zoomed out
            transform.position = Vector3.Lerp(transform.position, zoomedOutPosition, Time.deltaTime * moveSpeed);//smoothly moves the cam to the zoomed out pos
            transform.rotation = Quaternion.Lerp(transform.rotation, zoomedOutRotation, Time.deltaTime * rotateSpeed);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * fovSmoothSpeed);//puts the fov back to the scroll value if not zoomed out
            Vector3 targetPos = player.position + followOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);//moves the camera to follow the player with a appropriate offset
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), Time.deltaTime * rotateSpeed);
        }
    }
}
