using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Turrets/Shoot Configuration, order = 2")]
public class ShootConfigSO : ScriptableObject
{
    public LayerMask hitMask;
    public Vector3 spread;
    public Vector3 delay;
    public Vector3 burstDelay;
}