using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehabiour : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float gravity = -30.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private Vector3 velocity;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.5f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool isGrounded;
    
    [Header("Onscreen Controller")]
    [SerializeField] private Joystick leftJoystick;
    [SerializeField] private GameObject onScreenControls;
    [SerializeField] private GameObject miniMap;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                // turn onscreen controls on
                onScreenControls.SetActive(true);
                break;
            case RuntimePlatform.WebGLPlayer:
            case RuntimePlatform.WindowsEditor:
                // turn onscreen controls off
                miniMap.SetActive(true);
                onScreenControls.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.0f;
        }

        // foreach (var touch in Input.touches)
        // {
        //     Debug.Log(touch.position);
        // }

        // keyboard Input (fallback) + onscreen joystick
        float x = Input.GetAxis("Horizontal") + leftJoystick.Horizontal;
        float z = Input.GetAxis("Vertical") + leftJoystick.Vertical;

        // move
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * maxSpeed * Time.deltaTime);

        // jump
        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(!miniMap.activeInHierarchy);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    public void OnJumpButtonPressed()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    public void OnMapButtonPressed()
    {
        // toggle minimap
        miniMap.SetActive(!miniMap.activeInHierarchy);
    }
}
