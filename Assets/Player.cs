using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 Dir = new Vector3(h, 0, v);
        transform.position += Dir * moveSpeed * Time.deltaTime;
    }
}
