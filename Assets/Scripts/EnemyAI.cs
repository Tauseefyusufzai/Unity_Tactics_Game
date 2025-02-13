using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isMoving = false;
    private List<Tile> currentPath = new List<Tile>();
    private Transform player;
    private Animator animator;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObject == null)
        {
            Debug.LogError("❌ Enemy AI: No Player found in the scene!");
            return;
        }

        player = playerObject.transform;
        animator = GetComponent<Animator>();
        StartCoroutine(EnemyMovementRoutine());
    }


    IEnumerator EnemyMovementRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f); // Wait before moving

            if (!isMoving && player != null)
            {
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null)
        {
            Debug.LogError("❌ Enemy AI: Player reference is NULL!");
            return;
        }

        List<Tile> allTiles = new List<Tile>(FindObjectsOfType<Tile>());
        Tile startTile = allTiles.Find(t => t.transform.position == transform.position);
        Tile playerTile = allTiles.Find(t => t.transform.position == player.position);

        if (startTile == null || playerTile == null)
        {
            Debug.LogError($"❌ Enemy AI: Could not find start or target tile! Start: {startTile}, Target: {playerTile}");
            return;
        }

        // Find a path but remove the last tile (so enemy stops before reaching player)
        currentPath = Pathfinding.FindPath(startTile, playerTile, allTiles);

        if (currentPath.Count > 1)
        {
            currentPath.RemoveAt(currentPath.Count - 1); // Remove last step to avoid moving onto player's tile
            Debug.Log($"✅ Enemy AI: Moving toward player but stopping at ({currentPath[currentPath.Count - 1].x}, {currentPath[currentPath.Count - 1].y})");
            StartCoroutine(FollowPath());
        }
        else
        {
            Debug.Log("🚫 No valid path found!");
        }
    }


    IEnumerator FollowPath()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            Debug.Log("🚫 Enemy AI: No path to follow!");
            yield break;
        }

        isMoving = true;
        animator?.SetBool("Moving", true); 

        for (int i = 0; i < Mathf.Min(2, currentPath.Count); i++) 
        {
            Tile tile = currentPath[i];

            // ✅ Rotate towards movement direction
            Vector3 direction = (tile.transform.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                    yield return null;
                }
            }

            // ✅ Smooth movement using MoveTowards()
            Vector3 startPosition = transform.position;
            Vector3 endPosition = tile.transform.position;
            float moveDuration = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, Time.deltaTime * 5f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition; // Ensure exact position
        }

        isMoving = false;
        animator?.SetBool("Moving", false);
    }


}
