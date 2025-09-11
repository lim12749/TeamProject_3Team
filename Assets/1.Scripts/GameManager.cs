using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int CoinScore = 0;
    public TextMeshProUGUI Text;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("매니저 오류 발생");
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        CoinScore++;

        Text.text = CoinScore.ToString();
    }
}
