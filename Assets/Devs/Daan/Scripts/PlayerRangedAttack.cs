using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float roundsPerMinute = 120f;

    private float nextFireTime = 0f;
    private GridManager gridManager;

    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
    }

    void Update()
    {
        if (bulletPrefab == null || gridManager == null) return;

        // Check our RPM cooldown clock
        if (Time.time >= nextFireTime)
        {
            Vector3 targetDirection = Vector3.zero;

            // Listen strictly to the Arrow Keys for firing direction
            if (Input.GetKey(KeyCode.UpArrow)) targetDirection = Vector3.forward;
            if (Input.GetKey(KeyCode.DownArrow)) targetDirection = Vector3.back;
            if (Input.GetKey(KeyCode.LeftArrow)) targetDirection = Vector3.left;
            if (Input.GetKey(KeyCode.RightArrow)) targetDirection = Vector3.right;

            // If an arrow key is being pressed, execute the shot!
            if (targetDirection != Vector3.zero)
            {
                Shoot(targetDirection);
            }
        }
    }

    private void Shoot(Vector3 direction)
    {
        float cooldownSeconds = 60f / roundsPerMinute;
        nextFireTime = Time.time + cooldownSeconds;

        Debug.Log("Player fired a shot!");

        Vector3 spawnPosition = transform.position + (Vector3.up * 0.5f) + (direction * 0.6f);
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        Vector3 targetPoint = transform.position + (direction * 50f);

        // Grab the script from the bullet we just spawned
        BulletProjectile projectileScript = bullet.GetComponent<BulletProjectile>();
        if (projectileScript != null)
        {
            // FIX: Pass "Player" as the third argument so the bullet knows you shot it!
            projectileScript.Setup(targetPoint, damage, "Unit");
        }
    }
}
