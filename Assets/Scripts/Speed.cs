using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class SpeedDisplayQ3 : MonoBehaviour
{
    [Header("References")]
    public TMP_Text speedText;
    public TMP_Text peakSpeedText;
    public Rigidbody playerRigidbody;

    [Header("Settings")]
    public float colorLerpSpeed = 5f;

    private float peakSpeed = 0f;
    private Color targetColor = Color.green;
    private Color currentColor = Color.green;

    void Update()
    {
        if (playerRigidbody == null || speedText == null) return;

        // Calculate horizontal speed
        Vector3 horizontalVel = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
        float speed = horizontalVel.magnitude;

        // Update peak speed
        if (speed > peakSpeed)
            peakSpeed = speed;

        // Update text
        speedText.text = $"Speed: {speed:F1} m/s";
        if (peakSpeedText != null)
            peakSpeedText.text = $"Peak: {peakSpeed:F1} m/s";

        // Determine target color based on speed
        if (speed <= 10f) targetColor = Color.green;
        else if (speed <= 20f) targetColor = Color.yellow;
        else targetColor = Color.red;

        // Smoothly transition the color
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorLerpSpeed);
        speedText.color = currentColor;
    }

    public void ResetPeakSpeed()
    {
        peakSpeed = 0f;
    }
}
