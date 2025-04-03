using System;
using UnityEngine;

public class RamHandler : MonoBehaviour
{
    private IDamageable damageable;

    private void Awake()
    {
        // Retrieve the IDamageable component from the parent.
        damageable = transform.parent.GetComponent<IDamageable>();
        if (damageable == null)
        {
            Debug.LogError("No IDamageable component found on the parent object.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Retrieve the IDamageable component from the other colliding object.
        IDamageable otherDamageable = other.gameObject.GetComponent<IDamageable>();
        if (otherDamageable != null && damageable != null)
        {
            // Calculate damage values based on current health values.
            float damageToSelf = Mathf.Min(otherDamageable.CurrentHealth / 10, damageable.CurrentHealth / 10);
            float damageToOther = damageable.CurrentHealth;

            damageable.TakeDamage(damageToSelf);
            otherDamageable.TakeDamage(damageToOther);
        }
    }
}
