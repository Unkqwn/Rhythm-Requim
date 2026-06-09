using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform enemyTransform;
    [SerializeField] float moveInterval = 2f;
    [SerializeField] int attackRange = 3; // How many tiles away the enemy can shoot

    private GridManager gridManager;
    private EnemyVision enemyVision;
    private EnemyRangedAttack enemyRangedAttack; // Swapped to our new ranged component!
    private PlayerHealth playerTargetComponent;

    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        enemyVision = GetComponent<EnemyVision>();
        enemyRangedAttack = GetComponent<EnemyRangedAttack>(); // Grab the shooting script
        playerTargetComponent = FindAnyObjectByType<PlayerHealth>();

        if (enemyTransform == null) enemyTransform = this.transform;

        StartCoroutine(MovementRoutine());
    }

    IEnumerator MovementRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            if (enemyTransform != null && gridManager != null)
            {
                ExecuteEnemyTurn();
            }
        }
    }

    void ExecuteEnemyTurn()
    {
        int currentX = Mathf.RoundToInt(enemyTransform.position.x / gridManager.UnityGridSize);
        int currentZ = Mathf.RoundToInt(enemyTransform.position.z / gridManager.UnityGridSize);
        Vector2Int enemyGridPos = new Vector2Int(currentX, currentZ);

        // 1. Scan for the player anywhere in our line of sight up to 'attackRange' tiles away
        GameObject visiblePlayer = enemyVision.ScanForPlayerRanged(enemyGridPos, gridManager, attackRange);

        if (visiblePlayer != null)
        {
            // If player is in sight, freeze position and shoot!
            enemyRangedAttack.ShootTarget(visiblePlayer);
        }
        else if (playerTargetComponent != null)
        {
            // Otherwise, keep hunting across tiles
            MoveTowardsPlayer(enemyGridPos);
        }
    }

    void MoveTowardsPlayer(Vector2Int enemyGridPos)
    {
        int playerX = Mathf.RoundToInt(playerTargetComponent.transform.position.x / gridManager.UnityGridSize);
        int playerZ = Mathf.RoundToInt(playerTargetComponent.transform.position.z / gridManager.UnityGridSize);
        Vector2Int playerGridPos = new Vector2Int(playerX, playerZ);

        Vector2Int bestDirection = Vector2Int.zero;
        float shortestDistance = float.MaxValue;

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = enemyGridPos + dir;

            if (gridManager.IsTileWalkable(neighborPos))
            {
                float distanceToPlayer = Vector2Int.Distance(neighborPos, playerGridPos);

                if (distanceToPlayer < shortestDistance)
                {
                    shortestDistance = distanceToPlayer;
                    bestDirection = dir;
                }
            }
        }

        if (bestDirection != Vector2Int.zero)
        {
            Vector2Int targetGridPos = enemyGridPos + bestDirection;
            float targetHeight = gridManager.GetTileHeight(targetGridPos);

            enemyTransform.position = new Vector3(
                targetGridPos.x * gridManager.UnityGridSize,
                targetHeight,
                targetGridPos.y * gridManager.UnityGridSize
            );
        }
    }
}