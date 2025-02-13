using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Grid/Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    [Serializable]
    public class ObstacleRow
    {
        public List<bool> row = new List<bool>(new bool[10]); // Initialize row with 10 columns
    }

    public List<ObstacleRow> obstacles = new List<ObstacleRow>(); // List of 10 rows

    private void OnEnable()
    {
        if (obstacles == null || obstacles.Count == 0)
        {
            Debug.Log("Initializing ObstacleData...");
            obstacles = new List<ObstacleRow>();

            for (int i = 0; i < 10; i++)
            {
                obstacles.Add(new ObstacleRow()); // Ensure all rows exist
            }
        }
    }
}
