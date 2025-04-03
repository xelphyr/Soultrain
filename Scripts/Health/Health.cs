using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _health;
    [SerializeField] public Image healthBar; // Used for static UI like train

    private Animator animator;
    private NavMeshAgent agent;

    private HealthBarUI dynamicBar; // For enemies with pooled UI

    public float CurrentHealth { get => _health; private set => _health = value; }
    public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public event Action<float> OnTakeDamage;

    public event Action<Vector3> OnDeath;

    void OnEnable()
    {
        CurrentHealth = MaxHealth;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // If no static health bar assigned, assume dynamic bar needed
        if (healthBar == null && gameObject.tag != "Segment")
        {
            dynamicBar = HealthBarManager.Instance.GetHealthBar(transform);
        }
    }
    
    public void SetHealth(float newHealth)
    {
        MaxHealth = newHealth;
        CurrentHealth = MaxHealth;

        // Update UI
        if (healthBar != null)
        {
            healthBar.fillAmount = CurrentHealth / MaxHealth;
        }
        else if (dynamicBar != null)
        {
            dynamicBar.SetHealth(CurrentHealth / MaxHealth);
        }

        // Optionally: check for death instantly
        if (Mathf.Approximately(CurrentHealth, 0f))
        {
            OnDeath?.Invoke(transform.position);
            Die();
        }
    }
    
    public void TakeDamage(float damage)
    {
        float damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);
        CurrentHealth -= damageTaken;

        // Update either static or dynamic bar
        if (healthBar != null)
        {
            healthBar.fillAmount = CurrentHealth / MaxHealth;
        }
        else if (dynamicBar != null)
        {
            dynamicBar.SetHealth(CurrentHealth / MaxHealth);
        }

        if (!Mathf.Approximately(damageTaken, 0f))
        {
            OnTakeDamage?.Invoke(damageTaken);
        }

        if (Mathf.Approximately(CurrentHealth, 0f) && !Mathf.Approximately(damageTaken, 0f))
        {
            OnDeath?.Invoke(transform.position);
            Die();
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