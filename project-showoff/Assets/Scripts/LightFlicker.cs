using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.2f;

    public float flickerSpeed = 1.0f;

    private Light2D flickerLight;

    void Start()
    {
        flickerLight = GetComponent<Light2D>();

        if (flickerLight == null) print("Check light source");
    }

    void Update()
    {
        if (flickerLight)
        {
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
            flickerLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }
    }
}
