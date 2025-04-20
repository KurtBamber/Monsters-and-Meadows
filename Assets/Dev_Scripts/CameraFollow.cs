using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 5f;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, pos, smoothSpeed * Time.deltaTime);

        transform.position = smoothPos;
        transform.LookAt(target);
    }
}
