using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public Animator _ani;

    [Header("감지 범위 설정")]
    public float sightRange = 10f;
    public float attackRange = 2f;

    private bool playerInSightRange;
    private bool playerInAttackRange;

    private bool alreadyAttacked;
    public float timeBetweenAttacks = 1.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _ani = GetComponent<Animator>();
    }

    private void Update()
    {
        playerInSightRange = Vector3.Distance(transform.position, player.position) <= sightRange;
        playerInAttackRange = Vector3.Distance(transform.position, player.position) <= attackRange;

        if (!playerInSightRange && !playerInAttackRange) Idle();
        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInAttackRange && playerInSightRange) Attack();
    }

    private void Idle()
    {
        agent.SetDestination(transform.position);
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // 공격 애니메이션 재생
            if (_ani != null)
                _ani.SetTrigger("Attack");

            Debug.Log("💥 Enemy Attacks Player!");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
