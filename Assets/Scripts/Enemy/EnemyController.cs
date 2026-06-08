using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform enemyTransform;
    [SerializeField] float moveInterval = 2f;

    private GridManager gridManager;
    private EnemyVision enemyVision;
    private EnemyAttack enemyAttack;

    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();

        // Grab references to our decoupled components on the same GameObject
        enemyVision = GetComponent<EnemyVision>();
        enemyAttack = GetComponent<EnemyAttack>();

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
        // 1. Calculate current grid coordinate
        int currentX = Mathf.RoundToInt(enemyTransform.position.x / gridManager.UnityGridSize);
        int currentZ = Mathf.RoundToInt(enemyTransform.position.z / gridManager.UnityGridSize);
        Vector2Int currentGridPos = new Vector2Int(currentX, currentZ);

        // 2. Scan for Player using our vision script
        GameObject playerTarget = enemyVision.ScanForPlayer(currentGridPos, gridManager);

        if (playerTarget != null)
        {
            // If player is adjacent, ATTACK instead of moving!
            enemyAttack.AttackTarget(playerTarget);
        }
        else
        {
            // If player is not adjacent, move randomly
            MoveRandomly(currentGridPos);
        }
    }

    void MoveRandomly(Vector2Int currentGridPos)
    {
        Vector2Int randomDirection = directions[Random.Range(0, directions.Length)];
        Vector2Int targetGridPos = currentGridPos + randomDirection;

        if (gridManager.IsTileWalkable(targetGridPos))
        {
            float targetHeight = gridManager.GetTileHeight(targetGridPos);

            enemyTransform.position = new Vector3(
                targetGridPos.x * gridManager.UnityGridSize,
                targetHeight,
                targetGridPos.y * gridManager.UnityGridSize
            );
        }
    }
}