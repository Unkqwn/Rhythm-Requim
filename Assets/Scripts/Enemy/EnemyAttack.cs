using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;

    public void AttackTarget(GameObject target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} attacked {target.name}!");
        }
    }
}
