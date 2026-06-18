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

        // 1. ROTATE THE CHARACTER TOWARDS THE SHOOTING DIRECTION
        if (direction != Vector3.zero)
        {
            // Force the direction to stay flat on the Y axis so the character doesn't tilt up/down
            Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);

            // Create the rotation looking down the shooting path
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);

            // Apply the rotation to the player
            transform.rotation = targetRotation;
        }

        // 2. SPAWN THE BULLET (This stays the same as before)
        Vector3 spawnPosition = transform.position + (Vector3.up * 0.5f) + (direction * 0.6f);
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        // Grab your bullet script component (using whatever bullet setup you currently have active)
        PlayerBullet projectileScript = bullet.GetComponent<PlayerBullet>();
        if (projectileScript != null)
        {
            projectileScript.Setup(direction, damage);
        }
    }
}
