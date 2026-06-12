using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 targetPosition;
    private float bulletDamage;
    private string factionToIgnore; // Stores "Player" or "Enemy" depending on who shot it
    private bool isInitialized = false;

    // FIX: Added 'ownerTag' so the bullet knows who fired it!
    public void Setup(Vector3 targetPos, float damageValue, string ownerTag)
    {
        GridManager gridManager = FindAnyObjectByType<GridManager>();
        float gridSize = gridManager != null ? gridManager.UnityGridSize : 1f;

        float snappedX = Mathf.RoundToInt(targetPos.x / gridSize) * gridSize;
        float snappedZ = Mathf.RoundToInt(targetPos.z / gridSize) * gridSize;

        targetPosition = new Vector3(snappedX, transform.position.y, snappedZ);
        bulletDamage = damageValue;
        factionToIgnore = ownerTag; // Save the tag of the shooter
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (Collider col in colliders)
            {
                IDamageable damageable = col.GetComponent<IDamageable>();

                // FIX: Instead of hardcoding "Enemy", it ignores whatever faction shot it!
                if (damageable != null && !col.CompareTag(factionToIgnore))
                {
                    damageable.TakeDamage(bulletDamage);
                }
            }

            Destroy(gameObject);
        }
    }
}