using UnityEngine;
using TMPro; // Remove if using standard UI Text

public class TextBeatPulse : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;
    public TMP_Text titleText; // Change to 'public Text titleText' if not using TextMeshPro

    [Header("Pulse Settings")]
    public float baseFontSize = 72f;
    public float maxPulseIncrease = 20f;
    public float smoothing = 10f;
    public float sensitivity = 2f;

    private float[] _samples = new float[256];

    void Update()
    {
        // 1. Get the current audio loudness
        audioSource.GetOutputData(_samples, 0);
        float amplitude = 0f;
        foreach (float s in _samples) amplitude += Mathf.Abs(s);
        amplitude /= _samples.Length;

        // 2. Calculate the target font size
        float targetSize = baseFontSize + (amplitude * sensitivity * maxPulseIncrease);

        // 3. Smoothly transition to that size
        titleText.fontSize = Mathf.Lerp(titleText.fontSize, targetSize, Time.deltaTime * smoothing);
    }
}