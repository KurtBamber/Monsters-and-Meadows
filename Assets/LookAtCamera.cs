using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAtCamera : MonoBehaviour
{
    public Transform Camera;


    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.position);
    }
}
