using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCameraController : NetworkBehaviour
{
    // public values
    [SerializeField] private float ctrlSensitivity = 10.0f;
    [SerializeField] private Transform playerBody;

    // private values
    private float XRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput;
        float verticalInput;

        horizontalInput = Input.GetAxis("Mouse X") * ctrlSensitivity;
        verticalInput = Input.GetAxis("Mouse Y") * ctrlSensitivity;

        XRotation -= verticalInput;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        // rotating left / right
        transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * horizontalInput);
    }
}
