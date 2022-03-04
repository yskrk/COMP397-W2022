using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("TileResources")]
    [SerializeField] private List<GameObject> tilePrefabs;
    public GameObject startTile;
    public GameObject goalTile;

    [Header("TileProperties")]
    [Range(2, 30)]public int width = 2;
    [Range(2, 30)]public int depth = 2;
    [SerializeField] private Transform parent;

    [Header("Generated Tiles")]
    [SerializeField] private List<GameObject> tiles;
    [SerializeField] private int startWidth;
    [SerializeField] private int startDepth;

    // Start is called before the first frame update
    void Start()
    {
        startWidth = width;
        startDepth = depth;
        BuildMap();   
    }

    // Update is called once per frame
    void Update()
    {
        if (width != startWidth || depth != startDepth)
        {
            Resetmap();
            BuildMap();
        }
    }

    public void BuildMap()
    {
        var offset = new Vector3(16.0f, 0.0f, 16.0f);

        // place the start tile
        tiles.Add(Instantiate(startTile, Vector3.zero, Quaternion.identity, parent));

        // choose a random goal position, cannot be equal the start and cannot be larger than the grid
        var randomGoalRow = Random.Range(1, depth + 1);
        var randomGoalCol = Random.Range(1, width + 1);

        // gnerate more tiles if both width and depth are both greater than 2
        for (int row = 1; row <= depth; row++)
        {
            for (int col = 1; col <= width; col++)
            {
                if (row == 1 && col == 1) {continue;}

                var tilePosition = new Vector3(col * 16.0f, 0.0f, row * 16.0f) - offset;

                if (row == randomGoalRow && col == randomGoalCol)
                {
                    // place the goal tile
                    tiles.Add(Instantiate(goalTile, tilePosition, Quaternion.identity, parent));
                }
                else
                {
                    var randomPrefabIndex = Random.Range(0, 4);
                    var randomRotation = Quaternion.Euler(0.0f, Random.Range(0, 4) * 90, 0.0f);

                    tiles.Add(Instantiate(tilePrefabs[randomPrefabIndex], tilePosition, randomRotation, parent));
                }
            }
        }
    }

    public void Resetmap()
    {
        startWidth = width;
        startDepth = depth;
        var size = tiles.Count;

        for (int i = 0; i < size; i++)
        {
            Destroy(tiles[i]);
        }

        tiles.Clear();  // remove all tiles
    }
}
