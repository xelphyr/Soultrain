using System;
using UnityEngine;

public class RamHandler : MonoBehaviour
{
    public Health health;

    private void Awake()
    {
        health = transform.parent.GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Health otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth != null)
        {
            health.TakeDamage(Mathf.Min(otherHealth.health/10, health.health/10));
            otherHealth.TakeDamage(health.health);
        }
    }
}
