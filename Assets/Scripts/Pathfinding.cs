using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static List<Tile> FindPath(Tile startTile, Tile targetTile, List<Tile> allTiles)
    {
        if (startTile == null || targetTile == null)
        {
            Debug.LogError("❌ Error: Start Tile or Target Tile is NULL!");
            return new List<Tile>();
        }

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(startTile);

        while (openList.Count > 0)
        {
            Tile currentTile = openList[0];

            foreach (Tile tile in openList)
            {
                if (GetFCost(tile, startTile, targetTile) < GetFCost(currentTile, startTile, targetTile))
                    currentTile = tile;
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == targetTile)
            {
                return RetracePath(startTile, targetTile);
            }

            foreach (Tile neighbor in GetNeighbors(currentTile, allTiles))
            {
                if (neighbor == null)
                {
                    Debug.LogError($"❌ Error: Neighbor is NULL at ({currentTile.x}, {currentTile.y})");
                    continue;
                }

                if (neighbor.isObstacle) // Prevents from walking through obstacles
                {
                    Debug.Log($"🚫 Skipping obstacle at ({neighbor.x}, {neighbor.y})");
                    continue;
                }

                if (closedList.Contains(neighbor))
                    continue;

                if (!openList.Contains(neighbor))
                {
                    neighbor.parentTile = currentTile;
                    openList.Add(neighbor);
                }
            }

        }

        Debug.LogError("❌ Error: No Path Found!");
        return new List<Tile>();
    }


    private static List<Tile> GetNeighbors(Tile tile, List<Tile> allTiles)
    {
        List<Tile> neighbors = new List<Tile>();
        int[][] directions = { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };

        foreach (var dir in directions)
        {
            Tile neighbor = allTiles.Find(t => t.x == tile.x + dir[0] && t.y == tile.y + dir[1]);

            if (neighbor != null && !neighbor.isObstacle) // Only to add a walkable tiles
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }


    private static float GetFCost(Tile tile, Tile startTile, Tile targetTile)
    {
        float gCost = Mathf.Abs(tile.x - startTile.x) + Mathf.Abs(tile.y - startTile.y);  // Distance from start
        float hCost = Mathf.Abs(tile.x - targetTile.x) + Mathf.Abs(tile.y - targetTile.y); // Heuristic distance
        return gCost + hCost;
    }

    private static List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parentTile;
        }

        path.Reverse();
        return path;
    }
}
