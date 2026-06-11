using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        //  THE TRUTH LOG: This will print instantly when the player successfully hits the enemy
        Debug.LogWarning($" [ENEMY HIT!] {gameObject.name} took {damageAmount} damage! Health left: {currentHealth}");

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.LogError($" {gameObject.name} was destroyed!");
        Destroy(gameObject);
    }
}