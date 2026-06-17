using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform healthBarFill;

    public void SetHealthBar(float currentHealth)
    {
        if (healthBarFill == null)
        {
            Debug.LogWarning("Health bar fill is not assigned.");
            return;
        }
        float healthPercent = currentHealth / 100f;
        healthBarFill.localScale = new Vector3(healthPercent, 1f, 1f);
    }
}
