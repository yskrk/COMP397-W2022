using System.Runtime.CompilerServices;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class NetworkPlayerBehabiour : NetworkBehaviour
{
    // [SerializeField] private CharacterController controller;
    
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

    // position
    private NetworkVariable<float> remoteVerticalInput = new NetworkVariable<float>();
    private NetworkVariable<float> remoteHorizontalInput = new NetworkVariable<float>();
    private NetworkVariable<bool> remoteJumpInput = new NetworkVariable<bool>();

    // "local" values
    private float localHorizontalInput;
    private float localVerticalInput;
    private bool localJumpInput;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (!IsLocalPlayer)
        {
            GetComponentInChildren<NetworkCameraController>().enabled = false;
            GetComponentInChildren<Camera>().enabled = false;
        }

        if (IsServer)
        {
            RandomSpawnPositionAndColor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            // server update
            ServerUpdate();
        }

        if (IsOwner)
        {
            // client update
            ClientUpdate();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }        
    }

    private void LateUpdate()
    {
        if (IsLocalPlayer)
        {
            UpdateRotationYServerRPC(transform.eulerAngles.y);
        }
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.0f;
        }

        // move
        Vector3 move = transform.right * remoteHorizontalInput.Value + transform.forward * remoteVerticalInput.Value;
        GetComponent<CharacterController>().Move(move * maxSpeed * Time.deltaTime);

        // jump
        if (remoteJumpInput.Value && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
    }
    void ServerUpdate()
    {
        Move();
    }

    public void RandomSpawnPositionAndColor()
    {
        // player color
        var r = Random.Range(0, 1.0f);
        var g = Random.Range(0, 1.0f);
        var b = Random.Range(0, 1.0f);
        Color color = new Color(r, g, b);

        // player position
        var x = Random.Range(-10.0f, 10.0f);
        var z = Random.Range(-10.0f, 10.0f);
        transform.position = new Vector3(x, 1.0f, z);
    }

    public void ClientUpdate()
    {
        // keyboard input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool isJumping = Input.GetButton("Jump");

        // check if local variables have changed
        if (localHorizontalInput != x || localVerticalInput != z || localJumpInput != isJumping)
        {
            localHorizontalInput = x;
            localVerticalInput = z;
            localJumpInput = isJumping;

            // update the client position on the network
            UpdateClientPositionServerRpc(x, z, isJumping);
        }

        Move();
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float horizontal, float vertical, bool isJumping)
    {
        remoteHorizontalInput.Value = horizontal;
        remoteVerticalInput.Value = vertical;
        remoteJumpInput.Value = isJumping;
    }

    [ServerRpc]
    void UpdateRotationYServerRPC(float y)
    {
        transform.rotation = Quaternion.Euler(0.0f, y, 0.0f);
    }
}
