using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("적 리스트")]
    public List<GameObject> enemies = new List<GameObject>();

    [Header("플레이어 UI 캔버스 (씬에서 자동 찾기)")]
    public string playerUICanvasName = "PlayerUI";  // 씬에서 생성되는 캔버스 이름

    [Header("클리어 UI (씬에 있는 것)")]
    public GameObject clearCanvas;

    private GameObject playerUIInstance;
    private bool isCleared = false;

    void Start()
    {
        // 1. 씬 어디에서든 플레이어 UI 캔버스를 자동으로 찾기
        playerUIInstance = GameObject.Find(playerUICanvasName);

        if (playerUIInstance == null)
        {
            Debug.LogWarning("씬에서 Player UI Canvas를 찾지 못했습니다.");
        }

        // 클리어 UI는 비활성화
        if (clearCanvas != null)
            clearCanvas.SetActive(false);
    }

    void Update()
    {
        if (!isCleared)
            CheckEnemies();
    }

    void CheckEnemies()
    {
        enemies.RemoveAll(e => e == null);

        if (enemies.Count == 0)
        {
            isCleared = true;

            // 플레이어 UI 끄기
            if (playerUIInstance != null)
                playerUIInstance.SetActive(false);

            // 클리어 UI 켜기
            if (clearCanvas != null)
                clearCanvas.SetActive(true);

            Debug.Log("모든 적 처치 → Player UI OFF / Clear UI ON");
        }
    }
}
