using UnityEngine;
using UnityEngine.UI;

public class DestructibleHandler : MonoBehaviour
{
    public float maxHealth;
    float health;

    [Header("Healthbar Assets")]
    public Image healthBar;
    void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0.0001f)
        {   
            Die();
        }
        healthBar.fillAmount = health/maxHealth;
    }
    void Die()
    {
        Destroy(gameObject);
        return;
    }
}
