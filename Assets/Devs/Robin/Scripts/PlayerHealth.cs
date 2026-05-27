using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float health = 100f;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {health}");
        if (health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("Player has died.");
    }
}
