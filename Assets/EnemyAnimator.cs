using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // NavMeshAgent의 현재 속력을 이용해 Speed 파라미터 전달
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }
}
