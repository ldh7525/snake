using UnityEngine;

public enum BlockType
{
    Normal,
    Wall,
    Snake,
    Apple
}

public class Block : MonoBehaviour
{
    public BlockType type;
    [HideInInspector] public (int, int) gridPos;

    public void InitBlock(int x, int y)
    {
        gridPos = (x, y);
    }

    public void MoveBlockToThisGridPos((int, int) gridTuple)
    {
        transform.position = GridManager.Instance.GetPosByGrid(gridTuple);
    }

    public void SetGridAndMove((int, int) gridTuple)
    {
        gridPos = gridTuple;
        transform.position = GridManager.Instance.GetPosByGrid(gridTuple);
    }
}
