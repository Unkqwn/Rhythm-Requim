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

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Vector3 pointLow = new Vector3(transform.position.x, -5f, transform.position.z);
            Vector3 pointHigh = new Vector3(transform.position.x, 5f, transform.position.z);

            Collider[] colliders = Physics.OverlapCapsule(pointLow, pointHigh, 0.6f);

            foreach (Collider col in colliders)
            {
                // Strict rule: ONLY hurt objects tagged Player or Unit
                if (col.CompareTag("Player") || col.CompareTag("Unit"))
                {
                    IDamageable damageable = col.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(bulletDamage);
                        break;
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}