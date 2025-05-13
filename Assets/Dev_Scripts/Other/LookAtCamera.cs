using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAtCamera : MonoBehaviour
{

    public new Camera camera; // Camera transform to look at
    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position); // Rotates towards cameras position
    }
}
