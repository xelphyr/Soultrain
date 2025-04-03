using UnityEngine;
using System;
public interface IDamageable
{
    public float CurrentHealth { get;  }
    public float MaxHealth { get;  }

    public event Action<float> OnTakeDamage;
    public event Action<Vector3> OnDeath;

    public void TakeDamage(float damage);
}
