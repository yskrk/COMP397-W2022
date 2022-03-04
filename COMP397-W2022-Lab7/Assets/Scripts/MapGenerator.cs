using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("TileResources")]
    [SerializeField] private List<GameObject> tilePrefabs;

    [Header("TileProperties")]
    [Range(2, 30)][SerializeField] private int width = 2;
    [Range(2, 30)][SerializeField] private int depth = 2;
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
        // gnerate more tiles if both width and depth are both greater than 2
        var offset = new Vector3(16.0f, 0.0f, 16.0f);
        for (int row = 0; row < depth; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (row == 1 && col == 1) {continue;}
                var randomPrefabIndex = Random.Range(0, 4);
                var randomRotation = Quaternion.Euler(0.0f, Random.Range(0, 4) * 90, 0.0f);
                var tilePosition = new Vector3(col * 16.0f, 0.0f, row * 16.0f) - offset;

                tiles.Add(
                    Instantiate(
                        tilePrefabs[randomPrefabIndex], 
                        tilePosition, 
                        randomRotation, 
                        parent
                    )
                );
            }
        }
    }

    public void Resetmap()
    {
        startWidth = width;
        startDepth = depth;
        var tempTile = tiles[0];
        var size = tiles.Count;

        for (int i = 1; i < size; i++)
        {
            Destroy(tiles[i]);
        }

        tiles.Clear();  // remove all tiles
        tiles.Add(tempTile);
    }
}
