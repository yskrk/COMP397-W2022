using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMaker : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] private GameObject playerPrefab;

    [Header("World Properties")]
    [Range(1, 64)] [SerializeField] private int height = 1;
    [Range(1, 64)] [SerializeField] private int width = 1;
    [Range(1, 64)] [SerializeField] private int depth = 1;

    [Header("Scaling Values")]
    [SerializeField] private float min = 16.0f;
    [SerializeField] private float max = 24.0f;

    [Header("Tile Properties")]
    [SerializeField] private Transform tileParent;
    [SerializeField] private GameObject tile3D;

    [Header("Grid")]
    [SerializeField] private List<GameObject> grid;

    // starting values
    private int startHeight;
    private int startWidth;
    private int startDepth;
    private float startMin;
    private float startMax;


    // Start is called before the first frame update afeef
    void Start()
    {
        grid = new List<GameObject>();  // creates a new empty container

        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (height != startHeight || depth != startDepth || width != startDepth || min != startMin || max != startMax)
        {
            Generate();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Generate();
        }
    }

    private void Generate()
    {
        Initialized();
        Reset();
        Regenerate();
        PositionPlayer();
    }

    private void Initialized()
    {
        startHeight = height;
        startWidth = width;
        startDepth = depth;
        startMin = min;
        startMax = max;
    }

    private void Regenerate()
    {
        // generation
        // perlin noise texture that we will sample
        float rand = Random.Range(min, max);

        float offsetX = Random.Range(-1024.0f, 1024.0f);
        float offsetZ = Random.Range(-1024.0f, 1024.0f);

        for (int y = 0; y < height; y++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y < Mathf.PerlinNoise((x + offsetX) / rand, (z + offsetZ) / rand) * depth * 0.5f)
                    {
                        var tile = Instantiate(tile3D, new Vector3(x, y, z), Quaternion.identity);
                        tile.transform.parent = tileParent;
                        grid.Add(tile);
                    }
                }
            }
        }
    }

    private void Reset()
    {
        foreach (var tile in grid)
        {
            Destroy(tile);
        }

        grid.Clear();
    }

    private void PositionPlayer()
    {
        playerPrefab.GetComponent<CharacterController>().enabled = false;

        playerPrefab.transform.position = new Vector3(width * 0.5f, height + 10.0f, +depth * 0.5f);

        playerPrefab.GetComponent<CharacterController>().enabled = true;
    }
}
