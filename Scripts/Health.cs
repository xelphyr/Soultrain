using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public Image healthBar;  // Used for static UI like train

    private float _health;
    public float health => _health;
    private Animator animator;
    private NavMeshAgent agent;

    private HealthBarUI dynamicBar; // For enemies with pooled UI

    void Start()
    {
        Invoke(nameof(InitHealth), 0.1f);

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // If no static health bar assigned, assume dynamic bar needed
        if (healthBar == null && gameObject.tag != "Segment")
        {
            dynamicBar = HealthBarManager.Instance.GetHealthBar(transform);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(health, 0, maxHealth);

        // Update either static or dynamic bar
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
        else if (dynamicBar != null)
        {
            dynamicBar.SetHealth(health / maxHealth);
        }

        if (health <= 0.0001f)
        {
            Die();
        }
    }

    void InitHealth()
    {
        _health = maxHealth;

        // Initialize health bar fill
        if (healthBar != null)
        {
            healthBar.fillAmount = 1f;
        }
        else if (dynamicBar != null)
        {
            dynamicBar.SetHealth(1f);
        }
    }

    void Die()
    {
        if (animator != null)
        {
            if (agent != null) agent.enabled = false;

            animator.ResetTrigger("attack");
            animator.SetTrigger("die");

            // Return health bar to pool after delay
            if (dynamicBar != null)
            {
                HealthBarManager.Instance.ReturnHealthBar(dynamicBar);
            }

            Destroy(gameObject, 5f);
        }
        else
        {
            if (dynamicBar != null)
            {
                HealthBarManager.Instance.ReturnHealthBar(dynamicBar);
            }

            Destroy(gameObject);
        }
    }
}