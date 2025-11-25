using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private bool dead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        dead = true;

        // 여기에 죽는 애니메이션 넣기
        // GetComponent<Animator>().SetTrigger("Die");
    }

    public bool IsDead()
    {
        return dead;
    }
}
