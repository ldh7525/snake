using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Snake : MonoBehaviour
{
    private Tilemap tilemap => GridManager.Instance.tilemap;
    private int xSize => GridManager.Instance.xSize;
    private int ySize => GridManager.Instance.ySize;

    private TileBase appleTile => GridManager.Instance.appleTile;
    private TileBase wallTile => GridManager.Instance.wallTile;
    private TileBase floorTile => GridManager.Instance.floorTile;
    private TileBase snakeHeadTile => GridManager.Instance.snakeHeadTile;
    private TileBase snakeBodyTile => GridManager.Instance.snakeBodyTile;


    [SerializeField] private float moveInterval = 0.5f;
    private float timer;

    private List<Vector3Int> snakeBodyCoords;

    private Direction currentDirection;
    private Direction prevDirection;
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// 생성 후 초기화
    /// </summary>
    public void Init()
    {
        timer = 0f;

        Vector3Int firstPos = new Vector3Int(xSize / 2, ySize / 2, 0);
        snakeBodyCoords = new List<Vector3Int>();
        snakeBodyCoords.Add(firstPos);
        tilemap.SetTile(firstPos, snakeHeadTile);

        currentDirection = Direction.Right;
        MoveApple();
    }

    private Vector3Int currentApplePos;

    // 생성 & 이동 겸함
    private void MoveApple()
    {
        do
        {
            int x = Random.Range(1, xSize - 1);
            int y = Random.Range(1, ySize - 1);
            currentApplePos = new Vector3Int(x, y, 0);
        } while (tilemap.GetTile(currentApplePos) != floorTile);
        tilemap.SetTile(currentApplePos, appleTile);
    }

    private void Update()
    {
        // 방향바꾸기
        ChangeDirection();

        // movetimer
        if (timer > 0f)
            timer -= Time.deltaTime;
        else
        {
            Move();
            timer = moveInterval;
        }
    }

    private void Move()
    {
        Vector3Int nextHeadPos = GetNextHeadPos();
        TileBase nextTile = tilemap.GetTile(nextHeadPos);

        if (nextTile != floorTile && nextTile != appleTile)
        {
            GameManager.Instance.GameOver();
            return;
        }

        tilemap.SetTile(nextHeadPos, snakeHeadTile);
        snakeBodyCoords.Insert(0, nextHeadPos); // 리스트 맨 앞에 새 머리 좌표 추가

        // 사과 (있으면) 먹기
        bool ateApple = false;
        if (nextHeadPos == currentApplePos)
            ateApple = true;

        if (ateApple)
        {
            tilemap.SetTile(snakeBodyCoords[1], snakeBodyTile);
            MoveApple();
        }
        else
        {
            if (snakeBodyCoords.Count <= 2)
            {
                tilemap.SetTile(snakeBodyCoords[1], floorTile);
            }
            else
            {
                tilemap.SetTile(snakeBodyCoords[1], snakeBodyTile);
            }

            tilemap.SetTile(snakeBodyCoords[snakeBodyCoords.Count - 1], floorTile);
            snakeBodyCoords.RemoveAt(snakeBodyCoords.Count - 1);
        }

        prevDirection = currentDirection;
    }

    private Vector3Int GetNextHeadPos()
    {
        Vector3Int headPos = snakeBodyCoords[0];

        Vector3Int directionVector = currentDirection switch
        {
            Direction.Up => Vector3Int.up,
            Direction.Down => Vector3Int.down,
            Direction.Left => Vector3Int.left,
            Direction.Right => Vector3Int.right,
            _ => Vector3Int.zero
        };

        return headPos + directionVector;
    }

    private void ChangeDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) && prevDirection != Direction.Down)
            currentDirection = Direction.Up;
        else if (Input.GetKeyDown(KeyCode.S) && prevDirection != Direction.Up)
            currentDirection = Direction.Down;
        else if (Input.GetKeyDown(KeyCode.A) && prevDirection != Direction.Right)
            currentDirection = Direction.Left;
        else if (Input.GetKeyDown(KeyCode.D) && prevDirection != Direction.Left)
            currentDirection = Direction.Right;
    }
}