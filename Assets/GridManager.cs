using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Block blockPrefab;
    [SerializeField] private Block wallblockPrefab;

    public int xSize, ySize;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform blockParent;

    private float gridCellSize;

    Block[,] blocks; // x, y

    public void InitGrid()
    {
        gridCellSize = blockPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        blocks = new Block[xSize, ySize];

        for (int y = 0; y < ySize; y++)
            for (int x = 0; x < xSize; x++)
            {
                Block block;

                if (y == 0 || x == 0 || y == ySize - 1 || x == xSize - 1)
                    block = Instantiate(wallblockPrefab, GetPosByGrid(x, y), Quaternion.identity, blockParent);
                else
                    block = Instantiate(blockPrefab, GetPosByGrid(x, y), Quaternion.identity, blockParent);

                blocks[x, y] = block;
                block.InitBlock(x, y);
            }

    }

    // 오버로딩 해둠
    public Vector3 GetPosByGrid((int, int) gridTuple)
    {
        int x = gridTuple.Item1, y = gridTuple.Item2;
        return new Vector3(origin.position.x + (x + 1 / 2) * gridCellSize, origin.position.y + (y + 1 / 2) * gridCellSize, 0);
    }

    public Vector3 GetPosByGrid(int x, int y)
    {
        return new Vector3(origin.position.x + (x + 1 / 2) * gridCellSize, origin.position.y + (y + 1 / 2) * gridCellSize, 0);
    }

    public bool IsInsideWall((int, int) gridTuple)
    {
        int x = gridTuple.Item1, y = gridTuple.Item2;
        return x > 0 && x < xSize - 1 && y > 0 && y < ySize - 1;
    }
}
