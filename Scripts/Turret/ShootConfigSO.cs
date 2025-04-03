using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Turrets/Shoot Configuration", order = 2)]
public abstract class ShootConfigSO : ScriptableObject
{
    public LayerMask HitMask;

    public void Shoot(Stats stats)
    {
        
    }
}

public abstract class BulletShootConfigSO : ShootConfigSO
{
    public new void Shoot(Stats stats)
    {
        
    }
}