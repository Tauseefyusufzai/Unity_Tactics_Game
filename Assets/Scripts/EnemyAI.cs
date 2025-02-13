using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
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
            yield return new WaitForSeconds(1.5f); // Wait before moving

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

        currentPath = Pathfinding.FindPath(startTile, playerTile, allTiles);

        if (currentPath.Count > 1)
        {
            Debug.Log($"✅ Enemy AI: Found path! Moving from ({startTile.x},{startTile.y}) to ({playerTile.x},{playerTile.y})");
            StartCoroutine(FollowPath());
        }
        else
        {
            Debug.Log("❌ Enemy AI: No valid path found!");
        }
    }

    IEnumerator FollowPath()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            Debug.Log("❌ Enemy AI: No path to follow!");
            yield break;
        }

        isMoving = true;
        animator?.SetFloat("Speed", 5f); 

        for (int i = 0; i < Mathf.Min(2, currentPath.Count); i++) // Move up to 2 tiles
        {
            Tile tile = currentPath[i];

            // Rotate towards movement direction
            Vector3 direction = (tile.transform.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.8f);
            }

            transform.position = tile.transform.position;
            yield return new WaitForSeconds(0.2f);
        }

        animator?.SetFloat("Speed", 0f);
        isMoving = false;
    }

}
