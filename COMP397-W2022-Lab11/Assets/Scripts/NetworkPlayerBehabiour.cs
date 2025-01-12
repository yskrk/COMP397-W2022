using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class NetworkPlayerBehabiour : NetworkBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private MeshRenderer meshRenderer;

    // position
    private NetworkVariable<float> verticalPos = new NetworkVariable<float>();
    private NetworkVariable<float> horizontalPos = new NetworkVariable<float>();

    // color
    private NetworkVariable<Color> materialColor = new NetworkVariable<Color>();

    // "local" values
    private float localHorizontal;
    private float localVertical;
    private Color localColor;

    void Awake()
    {
        materialColor.OnValueChanged += ColorOnChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        RandomSpawnPositionAndColor();

        meshRenderer.material.SetColor("_Color", materialColor.Value);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            // server update
            ServerUpdate();
        }

        if (IsClient && IsOwner)
        {
            // client update
            ClientUpdate();
        }
        
    }

    void ServerUpdate()
    {
        transform.position = new Vector3(transform.position.x + horizontalPos.Value, 
            transform.position.y, 
            transform.position.z + verticalPos.Value);
    }

    public void RandomSpawnPositionAndColor()
    {
        // player color
        var r = Random.Range(0, 1.0f);
        var g = Random.Range(0, 1.0f);
        var b = Random.Range(0, 1.0f);
        Color color = new Color(r, g, b);
        localColor = color;

        // player position
        var x = Random.Range(-10.0f, 10.0f);
        var z = Random.Range(-10.0f, 10.0f);
        transform.position = new Vector3(x, 1.0f, z);
    }

    public void ClientUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        // network update
        if (localHorizontal != horizontal || localVertical != vertical)
        {
            localHorizontal = horizontal;
            localVertical = vertical;

            // update the client position on the network
            UpdateClientPositionServerRpc(horizontal, vertical);
        }

        if (localColor != materialColor.Value)
        {
            SetClientColorServerRpc(localColor);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float horizontal, float vertical)
    {
        horizontalPos.Value = horizontal;
        verticalPos.Value = vertical;
    }

    [ServerRpc]
    public void SetClientColorServerRpc(Color color)
    {
        materialColor.Value = color;

        if (meshRenderer.material.GetColor("_Color") != materialColor.Value)
        {
            meshRenderer.material.SetColor("_Color", materialColor.Value);
        }

    }

    void ColorOnChange(Color oldColor, Color newColor)
    {
        GetComponent<MeshRenderer>().material.color = materialColor.Value;
    }
}
