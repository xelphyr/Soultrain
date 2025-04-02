using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProperties", menuName = "Enemy")]
public class EnemyProperties : ScriptableObject
{
    [Header("Basic")]
    [SerializeField] public new string name;
    [SerializeField] public string description;

    [Header("Stats")]
    [SerializeField] public float health;
    [SerializeField] public float speed;
    [SerializeField] public float damage;

    [Header("Attack")]
    [SerializeField] public float attackRange;
    [SerializeField] public float attackCooldown;

}
