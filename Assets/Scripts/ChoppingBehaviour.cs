using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBehaviour : MonoBehaviour
{
    private GameObject targetResource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartChopping(GameObject resource)
    {
        targetResource = resource;
        Debug.Log("chop");
    }
}
