using UnityEngine;

public partial class EmergencyLight : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 150.0f;
    public Vector3 rotationAxis = Vector3.up;

    [Header("Intensity Pulse (Optional)")]
    public bool pulseIntensity = true;
    public float minIntensity = 2.0f;
    public float maxIntensity = 10.0f;
    public float pulseSpeed = 5.0f;

    private Light lightSource;

    void Start()
    {
        lightSource = GetComponent<Light>();
    }

    void Update()
    {
        // 1. Rotate the spotlight
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

        // 2. Pulse the light brightness if enabled
        if (pulseIntensity && lightSource != null)
        {
            float lerp = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
            lightSource.intensity = Mathf.Lerp(minIntensity, maxIntensity, lerp);
        }
    }
}