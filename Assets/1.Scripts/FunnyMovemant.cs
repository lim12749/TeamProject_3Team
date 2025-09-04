using UnityEngine;

public class FunnyMovemant : MonoBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0, v);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
