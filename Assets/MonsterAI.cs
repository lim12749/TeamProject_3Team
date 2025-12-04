using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;

    [Header("AI 설정")]
    public float chaseRange = 10f;      // 추격 시작 거리
    public float attackRange = 1.5f;    // 공격 거리
    public float stopChaseRange = 14f;  // 추격 중단 거리

    [Header("공격 설정")]
    public float timeBetweenAttacks = 1.5f;
    private bool alreadyAttacked;

    [Header("체력 설정")]
    public float health = 100f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 부드럽고 빠른 회전 설정
        agent.angularSpeed = 500f;
        agent.acceleration = 80f;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // 플레이어가 너무 멀면 Idle
        if (distance > stopChaseRange)
        {
            agent.isStopped = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", false);
            return;
        }

        // 공격 범위 바깥 → 추격
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }
        // 공격 범위 안 → 공격
        else
        {
            agent.isStopped = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                alreadyAttacked = true;
                Debug.Log("몬스터가 플레이어를 공격!");
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // ---------- 총알 충돌 처리 ----------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(50f);
            Destroy(other.gameObject);
        }
    }

    // ---------- 데미지 처리 ----------
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
            Die();
    }

    // ---------- 몬스터 죽음 ----------
    private void Die()
    {
        Debug.Log("몬스터 사망!");

        // 죽음 애니메이션이 있다면 재생 가능
        // animator.SetTrigger("Die");

        Destroy(gameObject); // 즉시 삭제
    }

    // ---------- 시각적 디버그 ----------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
