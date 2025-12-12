using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject snakePrefab;
    [SerializeField] Snake snake;

    [SerializeField] private TMP_Text text;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        GridManager.Instance.GenerateSnakeMap();
        SpawnSnake();
    }

    private void SpawnSnake()
    {
        snake.Init();
    }

    public void GameOver()
    {
        // 1. 시간 멈추기 (물리, 타이머 등 모든 업데이트 정지)
        Time.timeScale = 0f;

        // 2. UI 띄우기
        text.gameObject.SetActive(true);
    }
}
