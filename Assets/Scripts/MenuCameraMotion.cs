using UnityEngine;

public class MenuCameraMotion : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementAmount = 0.5f;
    [SerializeField] private float movementSpeed = 0.5f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationAmount = 1.0f;
    [SerializeField] private float rotationSpeed = 0.3f;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    void Start()
    {
        // Store the original position and rotation
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate a floating motion using Sine waves
        float xMove = Mathf.Sin(Time.time * movementSpeed) * movementAmount;
        float yMove = Mathf.Cos(Time.time * (movementSpeed * 1.2f)) * movementAmount;
        
        transform.position = _startPosition + new Vector3(xMove, yMove, 0);

        // Calculate a subtle tilt/rotation
        float xTilt = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        float yTilt = Mathf.Cos(Time.time * (rotationSpeed * 0.8f)) * rotationAmount;

        transform.rotation = _startRotation * Quaternion.Euler(xTilt, yTilt, 0);
    }
}