using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10, height = 10;  // Grid size
    public GameObject tilePrefab;  // Reference to the tile prefab

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                tile.name = $"Tile_{x}_{y}";
                
                Tile tileComponent = tile.AddComponent<Tile>(); 
                tileComponent.SetPosition(x, y);
            }
        }
    }
}
