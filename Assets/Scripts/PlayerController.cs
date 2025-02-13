using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isMoving = false;
    private List<Tile> currentPath = new List<Tile>();
    private Animator animator; // for animation

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveToTile(Tile targetTile, List<Tile> allTiles)
    {
        if (isMoving) return;

        // Round the player's position to match tile positions
        Vector3 roundedPosition = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z)
        );

        Tile startTile = allTiles.Find(t => t.transform.position == roundedPosition);

        if (startTile == null)
        {
            Debug.LogError($"❌ Error: Player is not on a valid tile at {roundedPosition}!");
            return;
        }

        currentPath = Pathfinding.FindPath(startTile, targetTile, allTiles);

        if (currentPath.Count > 0)
            StartCoroutine(FollowPath());
        else
            Debug.LogError("❌ Error: No valid path found!");
    }
    IEnumerator FollowPath()
    {
        isMoving = true;
        animator.SetFloat("Speed", 5f); 

        for (int i = 0; i < currentPath.Count; i++)
        {
            Tile tile = currentPath[i];

            // Rotate the player towards movement direction
            Vector3 direction = (tile.transform.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.9f);
            }

            transform.position = tile.transform.position;
            yield return new WaitForSeconds(0.2f);
        }

        animator.SetFloat("Speed", 0f);
        isMoving = false;
    }
}
