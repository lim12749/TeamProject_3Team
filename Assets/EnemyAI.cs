using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float chaseRange = 5f;

    private NavMeshAgent navMeshAgent;
    private EnemyHealth health;

    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        // ★ 죽었을 때 AI 끄기 — 중요
        if (health.IsDead())
        {
            enabled = false;              // EnemyAI 스크립트 자체 비활성화
            navMeshAgent.enabled = false; // NavMeshAgent 멈춤 (or isStopped 사용 가능)
            return;
        }

        // ★ 플레이어까지 거리 계산
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true; // 추적 시작
        }
    }


    private void EngageTarget()
    {
        // ★ 추적 (제동 거리보다 멀면 이동)
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        // ★ 공격 (제동 거리 안에 들어오면 공격)
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        // 공격 애니메이션, 데미지 처리 등 들어가는 곳
        Debug.Log("Attack!");
    }
}
