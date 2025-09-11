using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed = 2f;      // 이동 속도
    public float leftLimit = -5f;     // 왼쪽 이동 한계
    public float rightLimit = 5f;     // 오른쪽 이동 한계

    private bool movingRight = true;  // 이동 방향

    private void Update()
    {
        // 몬스터 이동
        MoveMonster();
    }

    private void MoveMonster()
    {
        // 이동 방향에 따라 몬스터 위치 변경
        if (movingRight)
        {
            // MoveTowards를 사용해서 부드럽게 이동
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(rightLimit, transform.position.y, transform.position.z),
                moveSpeed * Time.deltaTime);

            // 오른쪽 한계에 도달하면 방향 변경
            if (transform.position.x >= rightLimit)
            {
                movingRight = false;
            }
        }
        else
        {
            // MoveTowards를 사용해서 부드럽게 이동
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(leftLimit, transform.position.y, transform.position.z),
                moveSpeed * Time.deltaTime);

            // 왼쪽 한계에 도달하면 방향 변경
            if (transform.position.x <= leftLimit)
            {
                movingRight = true;
            }
        }
    }
}
