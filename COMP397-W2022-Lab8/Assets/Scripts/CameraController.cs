using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // public values
    [SerializeField] private float mouseSensitivity = 10.0f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Joystick rightstick;

    // private values
    private float XRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity + rightstick.Horizontal;
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity + rightstick.Vertical;

        float mouseX = rightstick.Horizontal;
        float mouseY = rightstick.Vertical;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        // rotating left / right
        transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
