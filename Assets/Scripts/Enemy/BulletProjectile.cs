using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 targetPosition;
    private float bulletDamage;
    private bool isInitialized = false;

    public void Setup(Vector3 targetPos, float damageValue)
    {
        GridManager gridManager = FindAnyObjectByType<GridManager>();
        float gridSize = gridManager != null ? gridManager.UnityGridSize : 1f;

        float snappedX = Mathf.RoundToInt(targetPos.x / gridSize) * gridSize;
        float snappedZ = Mathf.RoundToInt(targetPos.z / gridSize) * gridSize;

        targetPosition = new Vector3(snappedX, transform.position.y, snappedZ);
        bulletDamage = damageValue;
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;

        // Move strictly toward the fixed snapped tile position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check for arrival/impact
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (Collider col in colliders)
            {
                IDamageable damageable = col.GetComponent<IDamageable>();
                if (damageable != null && !col.CompareTag("Enemy"))
                {
                    damageable.TakeDamage(bulletDamage);
                }
            }

            Destroy(gameObject);
        }
    }
}