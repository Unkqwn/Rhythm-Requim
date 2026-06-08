using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    public GameObject ScanForPlayer(Vector2Int enemyGridPos, GridManager gridManager)
    {
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player == null) return null;

        int playerX = Mathf.RoundToInt(player.transform.position.x / gridManager.UnityGridSize);
        int playerZ = Mathf.RoundToInt(player.transform.position.z / gridManager.UnityGridSize);
        Vector2Int playerGridPos = new Vector2Int(playerX, playerZ);

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = enemyGridPos + dir;

            if (neighborPos == playerGridPos)
            {
                Debug.Log($"Enemy detected Player mathematically at adjacent grid slot: {playerGridPos}!");
                return player.gameObject; // Target found
            }
        }

        return null; // Player is not next to the enemy
    }
}