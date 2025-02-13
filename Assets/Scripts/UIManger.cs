using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI tileInfoText;  

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Tile clickedTile = hit.collider.GetComponent<Tile>();

                if (clickedTile != null && !clickedTile.isObstacle)
                {
                    PlayerController player = FindObjectOfType<PlayerController>();
                    List<Tile> allTiles = new List<Tile>(FindObjectsOfType<Tile>());

                    if (allTiles.Count == 0)
                    {
                        Debug.LogError("❌ Error: No Tiles Found in Scene!");
                        return;
                    }

                    Debug.Log($"✅ Moving to Tile: ({clickedTile.x}, {clickedTile.y})");
                    player.MoveToTile(clickedTile, allTiles);
                }
            }
        }
    }
}
