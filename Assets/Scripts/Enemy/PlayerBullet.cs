using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float maxLifetime = 3f;

    private Vector3 moveDirection;
    private float bulletDamage;
    private bool isInitialized = false;

    public void Setup(Vector3 direction, float damageValue)
    {
        // Save the raw direction vector (like Vector3.forward, Vector3.right)
        moveDirection = direction.normalized;
        bulletDamage = damageValue;
        isInitialized = true;

        // Auto-destruct after 3 seconds so scene doesn't get cluttered
        Destroy(gameObject, maxLifetime);
    }

    void Update()
    {
        if (!isInitialized) return;

        // Keep moving forward smoothly down the lane every single frame
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    // ?? THE AUTOMATIC PHYSICS SENSOR
    // Unity calls this automatically the exact millisecond the bullet intersects any collider!
    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the object we collided with is tagged Enemy
        if (other.CompareTag("Enemy") || (other.transform.parent != null && other.transform.parent.CompareTag("Enemy")))
        {
            // 2. Try to find the health script
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable == null)
            {
                damageable = other.GetComponentInParent<IDamageable>();
            }

            // 3. Apply damage and explode!
            if (damageable != null)
            {
                damageable.TakeDamage(bulletDamage);
                Destroy(gameObject); // Destroy the bullet on impact
            }
        }
    }
}