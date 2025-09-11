using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    
    
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("충돌 감지");

        if (Input.GetKey(KeyCode.E))
        {
            GameManager.instance.AddCoin();
            Destroy(gameObject);
        }
    }
}
