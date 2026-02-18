using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        Quaternion targetRotCam = Quaternion.Euler(xRotation, 0f, 0f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotCam, 0.8f);

        Quaternion targetRotBody = playerBody.rotation * Quaternion.Euler(0f, mouseX, 0f);
        playerBody.rotation = Quaternion.Lerp(playerBody.rotation, targetRotBody, 0.8f);
    }
}
