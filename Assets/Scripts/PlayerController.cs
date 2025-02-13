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
           if (animator == null)
            {
                Debug.LogError("❌ Animator component missing on Player!");
            }
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
        animator?.SetBool("Moving", true); //

        for (int i = 0; i < currentPath.Count; i++)
        {
            Tile tile = currentPath[i];

            // ✅ Rotate towards movement direction
            Vector3 direction = (tile.transform.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.8f);
            }

            // ✅ Smooth movement using Lerp
            Vector3 startPosition = transform.position;
            Vector3 endPosition = tile.transform.position;
            float elapsedTime = 0;
            float moveDuration = 0.5f; // Adjust for smoothness

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition; // Ensure exact position
        }
        isMoving = false;
        animator?.SetBool("Moving", false);
    }

}
