using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;
    public GameObject HumanMale_Character_Free;  
    public GameObject HumanMale_Character_Free_Enemy;  

    private List<Vector3> occupiedTiles = new List<Vector3>(); // Track the occupied positions


    void Start()
    {
        Debug.Log("ObstacleManager Started!");
        ResetObstacleData();  // Clear the previous obstacles
        GenerateObstacles();  // Generate the new random obstacles
        SpawnPlayer(); 
        SpawnEnemy();
    }

    void GenerateObstacles()
    {
        Debug.Log("Generating Obstacles...");

        List<Tile> allTiles = new List<Tile>(FindObjectsOfType<Tile>());

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Tile tile = allTiles.Find(t => t.x == x && t.y == y);

                if (tile == null)
                {
                    Debug.LogError($"❌ Error: Tile at ({x}, {y}) not found!");
                    continue;
                }

                bool isObstacle = Random.value < 0.2f; // 20% chance of being an obstacle
                tile.isObstacle = isObstacle;

                if (isObstacle)
                {
                    Instantiate(obstaclePrefab, new Vector3(x, 0.5f, y), Quaternion.identity);
                    Debug.Log($"✅ Obstacle placed at ({x}, {y})");
                }
            }
        }
    }



    void ResetObstacleData()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                obstacleData.obstacles[y].row[x] = false; // Clear previous data
            }   
        }
    }
    void SpawnPlayer()
    {
        List<Tile> allTiles = new List<Tile>(FindObjectsOfType<Tile>());

        // Find a random walkable tile (not occupied)
        Tile spawnTile = allTiles.Find(t => !t.isObstacle);

        if (spawnTile != null)
        {
            Vector3 spawnPosition = spawnTile.transform.position;
            GameObject playerInstance = Instantiate(HumanMale_Character_Free, spawnPosition, Quaternion.identity);
            playerInstance.tag = "Player"; 
            occupiedTiles.Add(spawnPosition);
            Debug.Log($"✅ Player spawned at ({spawnTile.x}, {spawnTile.y})");
        }
        else
        {
            Debug.LogError("❌ Error: No valid tile found for Player Spawn!");
        }
    }

    void SpawnEnemy()
    {
        List<Tile> allTiles = new List<Tile>(FindObjectsOfType<Tile>());

        // Find a random walkable tile (not occupied by player)
        Tile spawnTile = allTiles.Find(t => !t.isObstacle && !occupiedTiles.Contains(t.transform.position));

        if (spawnTile != null)
        {
            Vector3 spawnPosition = spawnTile.transform.position;
            Instantiate(HumanMale_Character_Free_Enemy, spawnPosition, Quaternion.identity);
            occupiedTiles.Add(spawnPosition);
            Debug.Log($"✅ Enemy spawned at ({spawnTile.x}, {spawnTile.y})");
        }
        else
        {
            Debug.LogError("❌ Error: No valid tile found for Enemy Spawn!");
        }
    }
}
