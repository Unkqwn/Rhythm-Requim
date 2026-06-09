using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float damage = 15f;

    public void ShootTarget(GameObject target)
    {
        Debug.Log($"{gameObject.name} fires a shot at {target.name}!");

        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate (bulletPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity );

            BulletProjectile projectileScript = bullet.GetComponent<BulletProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Setup(target.transform.position, damage);
            }
        }
    }
}
