using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    //[SerializeField] private PlayerHealthBar healthBar;
    private float currentHealth = 100f;

    void Start()
    {
        currentHealth = maxHealth;
        //healthBar.SetHealthBar(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //healthBar.SetHealthBar(currentHealth);
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("Player has died.");
    }
}
