using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;  // Grid position
    public bool isObstacle = false;  // Whether this tile is walkable
    public Tile parentTile;  // Used for pathfinding

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
