using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject snakePrefab;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        GridManager.Instance.InitGrid();
        SpawnSnake();
    }

    private void SpawnSnake()
    {
        // ½ºÆù
        Snake snake = Instantiate(snakePrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Snake>();
        snake.Init();
    }

    public void GameOver()
    {
        Debug.Log("Fuck");
    }
}
