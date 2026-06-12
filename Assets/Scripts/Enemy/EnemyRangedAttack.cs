using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float roundsPerMinute = 60f;
    [SerializeField] private int attackRange = 3;

    private float nextFireTime = 0f;
    private GridManager gridManager;
    private EnemyVision enemyVision;

    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        enemyVision = GetComponent<EnemyVision>();
    }

    void Update()
    {
        if (gridManager == null || enemyVision == null || bulletPrefab == null) return;

        // Check if the gun itself is ready to fire based on RPM
        if (Time.time >= nextFireTime)
        {
            // Calculate current grid position
            int currentX = Mathf.RoundToInt(transform.position.x / gridManager.UnityGridSize);
            int currentZ = Mathf.RoundToInt(transform.position.z / gridManager.UnityGridSize);
            Vector2Int enemyGridPos = new Vector2Int(currentX, currentZ);

            // Scan to see if player is in sight lines
            GameObject visiblePlayer = enemyVision.ScanForPlayerRanged(enemyGridPos, gridManager, attackRange);

            if (visiblePlayer != null)
            {
                ShootTarget(visiblePlayer);
            }
        }
    }

    private void ShootTarget(GameObject target)
    {
        Debug.Log($"{gameObject.name} fires a shot at {target.name}!");

        // Set the next allowed fire time cleanly based on RPM math
        float cooldownSeconds = 60f / roundsPerMinute;
        nextFireTime = Time.time + cooldownSeconds;

        GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        BulletProjectile projectileScript = bullet.GetComponent<BulletProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Setup(target.transform.position, damage, "Enemy");
        }
    }

    // Public helper so the movement brain knows if the gun is currently shooting someone
    public bool IsPlayerInSight()
    {
        if (gridManager == null || enemyVision == null) return false;

        int currentX = Mathf.RoundToInt(transform.position.x / gridManager.UnityGridSize);
        int currentZ = Mathf.RoundToInt(transform.position.z / gridManager.UnityGridSize);
        Vector2Int enemyGridPos = new Vector2Int(currentX, currentZ);

        return enemyVision.ScanForPlayerRanged(enemyGridPos, gridManager, attackRange) != null;
    }
}