using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Turrets/Shoot Configuration, order = 2")]
public class ShootConfigSO : ScriptableObject
{
    public LayerMask HitMask;
    public Vector3 Spread;
    public float FireRate = 0.25f;
    public float BurstRate = 0.5f;
}