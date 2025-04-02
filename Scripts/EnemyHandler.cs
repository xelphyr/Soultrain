using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public EnemyProperties properties;

    float damage;
    float attackRange;
    float attackCooldown;

    bool alreadyAttacked;
    float cooldown;
    bool playerInAttackRange;

    Health health;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        damage = properties.damage;
        attackRange = properties.attackRange;
        attackCooldown = properties.attackCooldown;

        agent.speed = properties.speed;
        agent.acceleration = properties.speed * 1.5f;
        health = gameObject.GetComponent<Health>();
        health.maxHealth = properties.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Engine(Clone)").transform;
            agent = GetComponent<NavMeshAgent>();
            return;
        }

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        Debug.Log(properties.name + ": " + playerInAttackRange);

        if (!playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();

        animator.SetFloat("speed", agent.velocity.magnitude);
        
        if (alreadyAttacked)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0f)
            {
                alreadyAttacked = false;
            }
        }
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        Debug.Log("Attack2");
        if (!alreadyAttacked)
        {
            Debug.Log("Attack3");
            agent.SetDestination(transform.position);
            Debug.Log("Attack4");
            transform.LookAt(player.position);
            Debug.Log("Attack5");
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die")) animator.SetTrigger("attack");
            Debug.Log("Attack6");
            alreadyAttacked = true;
            Debug.Log("Attack7");
            cooldown = attackCooldown;
            Debug.Log("Attack8");
            player.parent.GetComponent<Health>().TakeDamage(damage);
            Debug.Log("Attack9");
        }
    }
}
