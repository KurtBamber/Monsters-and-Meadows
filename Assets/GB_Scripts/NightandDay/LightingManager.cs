using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    // Link to Video Walkthrough https://youtu.be/m9hj9PdO328?si=UpKsqznVFBxYVGnX

    // References
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset Preset;
    // Variables
    [SerializeField, Range(0, 24)] public float timeOfDay;

    private void Update()
    {

        if(Preset == null) 
            return;

        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime / 3;
            timeOfDay %= 24; // Clamp time to 0 - 1 for gradient
            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.ambientColour.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.fogColour.Evaluate(timePercent);

        if(directionalLight != null)
        {
            directionalLight.color = Preset.directionalColour.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}
