using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 8f;      // 추격 시작 거리
    public float attackRange = 1.5f;   // 공격 거리
    public float stopChaseRange = 12f; // 너무 멀면 Idle로

    private Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // 1. 너무 멀면 Idle
        if (distance > stopChaseRange)
        {
            agent.isStopped = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", false);
            return;
        }

        // 2. 공격 범위 밖이면 Walk 상태
        if (distance > attackRange && distance <= stopChaseRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }

        // 3. 공격 범위 안이면 Attack
        if (distance <= attackRange)
        {
            agent.isStopped = true;

            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);

            // 여기서 플레이어 체력 깎는 부분 실행
            // player.GetComponent<PlayerHP>().TakeDamage(1);
        }
    }
}
