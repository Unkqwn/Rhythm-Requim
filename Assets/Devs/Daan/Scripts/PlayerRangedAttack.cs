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
        // Set the next allowed fire time cleanly based on RPM math
        float cooldownSeconds = 60f / roundsPerMinute;
        nextFireTime = Time.time + cooldownSeconds;

        Debug.Log("Player fired a shot!");

        // Spawn the player bullet slightly out in front of the player so it doesn't collide with yourself
        Vector3 spawnPosition = transform.position + (Vector3.up * 0.5f) + (direction * 0.6f);
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        // Calculate a target point far down the line on the grid map
        Vector3 targetPoint = transform.position + (direction * 50f);


    }
}
