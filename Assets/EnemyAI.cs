using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("AI Ranges")]
    [SerializeField] private float chaseRange = 5f;      // 감지 범위
    [SerializeField] private float attackRange = 1.5f;   // 공격 범위

    [Header("Animator")]
    [SerializeField] private Animator animator;          // Animator 연결

    private NavMeshAgent navMeshAgent;
    private EnemyHealth health;

    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (health.IsDead())
        {
            enabled = false;
            navMeshAgent.enabled = false;
            animator.SetFloat("Speed", 0f); // 멈춤
            animator.SetBool("Attack", false);
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        // 감지 범위 체크
        if (!isProvoked && distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }

        if (isProvoked)
        {
            EngageTarget();
        }
        else
        {
            animator.SetFloat("Speed", 0f); // Idle
            animator.SetBool("Attack", false);
        }
    }

    private void EngageTarget()
    {
        if (distanceToTarget > attackRange)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.position);

        // ⭐ Speed 정규화
        float normalizedSpeed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        animator.SetFloat("Speed", normalizedSpeed);  // 0~1 범위
        animator.SetBool("Attack", false);
    }

    private void AttackTarget()
    {
        navMeshAgent.isStopped = true;
        animator.SetFloat("Speed", 0f); // 멈춤
        animator.SetBool("Attack", true);
        Debug.Log("Attack!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange); // 감지 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위
    }
}
