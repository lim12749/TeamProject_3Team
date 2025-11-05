using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Tooltip("초기 체력")]
    public float maxHealth = 100f;

    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    // IDamageable 구현
    public bool TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        if (currentHealth <= 0f)
        {
            Die();
            return true;
        }
        return false;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }
}

