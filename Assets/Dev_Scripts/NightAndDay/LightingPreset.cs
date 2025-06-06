using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="Lighting Preset", menuName ="Scriptables/Lighting Preset",order =1)]
public class LightingPreset : ScriptableObject
{
    public Gradient ambientColour;
    public Gradient directionalColour;
    public Gradient fogColour;
}
