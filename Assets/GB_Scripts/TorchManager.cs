using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TorchManager : MonoBehaviour
{
    public Gradient torchLighting;
    private GameObject torchParent;
    public Light[] Torches;

    public LightingManager LM;
    public GameObject lightManager;

    public void Start()
    {
       LM = lightManager.GetComponent<LightingManager>();

       torchParent = this.gameObject;
       Torches = torchParent.GetComponentsInChildren<Light>(); 
    }

    public void Update()
    {
        if (Application.isPlaying)
        {
            LM.timeOfDay += Time.deltaTime / 3;
            LM.timeOfDay %= 24; // Clamp time to 0 - 1 for gradient
            UpdateTorches(LM.timeOfDay / 24f);
        }
        else
        {
            UpdateTorches(LM.timeOfDay / 24f);
        }

        
    }

    public void UpdateTorches(float timePercent)
    {
        for (int i = 0; i < Torches.Length; i++)
        {
            Torches[i].color = torchLighting.Evaluate(timePercent);
        }
    }
}
