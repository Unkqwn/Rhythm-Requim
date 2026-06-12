using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    public GameObject ScanForPlayerRanged(Vector2Int enemyGridPos, GridManager gridManager, int attackRange)
    {
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player == null) return null;

        int playerX = Mathf.RoundToInt(player.transform.position.x / gridManager.UnityGridSize);
        int playerZ = Mathf.RoundToInt(player.transform.position.z / gridManager.UnityGridSize);
        Vector2Int playerGridPos = new Vector2Int(playerX, playerZ);

        // Check each direction outward up to the max attackRange
        foreach (Vector2Int dir in directions)
        {
            for (int i = 1; i <= attackRange; i++)
            {
                Vector2Int targetCheckPos = enemyGridPos + (dir * i);

                // If the player is on this tile, we found them!
                if (targetCheckPos == playerGridPos)
                {
                    return player.gameObject;
                }

                // Optional SOLID feature: If a wall blocks the tile, stop looking further in this line
                if (!gridManager.IsTileWalkable(targetCheckPos))
                {
                    break;
                }
            }
        }

        return null;
    }
}