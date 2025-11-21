using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject tailParent;
    [SerializeField] private GameObject tailBlockPrefab; // 걍 색만 바꾼거
    [SerializeField] private GameObject appleBlockPrefab; // 걍 색만 바꾼거

    [SerializeField] private float moveInterval = 0.5f;

    private float timer;

    private (int, int) HeadGridPos => headBlock.gridPos;

    private Block headBlock;
    private List<Block> tailBlocks;

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
        headBlock = head.GetComponent<Block>();
        headBlock.gridPos = (GridManager.Instance.xSize / 2, GridManager.Instance.ySize / 2);
        headBlock.MoveBlockToThisGridPos(HeadGridPos);

        timer = 0f;
        tailBlocks = new List<Block>();

        currentDirection = Direction.Right;

        InitApple();
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
        // 벽인지 플레이어몸인지 검사
        (int, int) nextHeadPos = GetNextHeadPos();
        if (!GridManager.Instance.IsInsideWall(nextHeadPos))
        {
            GameManager.Instance.GameOver();
            return;
        }

        for (int i = 0; i < tailBlocks.Count; i++)
        {
            if (nextHeadPos == tailBlocks[i].gridPos)
            {
                GameManager.Instance.GameOver();
                return;
            }
        }

        bool ateApple = false;
        // 사과 (있으면) 먹기
        if (nextHeadPos == CurrentAppleGridPos)
        {
            ExtendTail();
            ateApple = true;
        }

        // 테일 이동
        for (int i = tailBlocks.Count - 1; i >= 0; i--)
        {
            if (i == 0)
                tailBlocks[i].SetGridAndMove(HeadGridPos);
            else
                tailBlocks[i].SetGridAndMove(tailBlocks[i - 1].gridPos);
        }

        // 헤드 이동
        headBlock.gridPos = nextHeadPos;
        headBlock.MoveBlockToThisGridPos(HeadGridPos);

        prevDirection = currentDirection;

        // 사과 먹었으면 이동
        if (ateApple)
            MoveApple();
    }


    private (int, int) GetNextHeadPos()
    {
        switch (currentDirection)
        {
            case Direction.Up:
                return (HeadGridPos.Item1, HeadGridPos.Item2 + 1);
            case Direction.Down:
                return (HeadGridPos.Item1, HeadGridPos.Item2 - 1);
            case Direction.Left:
                return (HeadGridPos.Item1 - 1, HeadGridPos.Item2);
            case Direction.Right:
                return (HeadGridPos.Item1 + 1, HeadGridPos.Item2);
            default:
                throw new System.Exception("Invalid direction");
        }
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


    private (int, int) CurrentAppleGridPos => appleBlock.gridPos;
    private Block appleBlock;

    private void InitApple()
    {
        appleBlock = Instantiate(appleBlockPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Block>();
        MoveApple();
    }

    private bool IsOverlapWithSnakeBody((int, int) targetApplePosTuple)
    {
        if (targetApplePosTuple == HeadGridPos)
            return true;

        for (int i = 0; i < tailBlocks.Count; i++)
            if (targetApplePosTuple == tailBlocks[i].gridPos)
                return true;

        return false;
    }

    private void MoveApple()
    {
        (int, int) appleGridTarget;
        do
        {
            appleGridTarget = (Random.Range(1, GridManager.Instance.xSize - 1), Random.Range(1, GridManager.Instance.ySize - 1));
        }
        while (IsOverlapWithSnakeBody(appleGridTarget));

        appleBlock.SetGridAndMove(appleGridTarget);
    }

    // 사과먹음
    private void ExtendTail()
    {
        Block newTailBlock = Instantiate(tailBlockPrefab, Vector3.zero, Quaternion.identity, tailParent.transform).GetComponent<Block>();

        if (tailBlocks.Count == 0)
            newTailBlock.SetGridAndMove(HeadGridPos);
        else
            newTailBlock.SetGridAndMove(tailBlocks[tailBlocks.Count - 1].gridPos);

        tailBlocks.Add(newTailBlock);
    }
}