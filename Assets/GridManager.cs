using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Singleton<GridManager>
{
    public Tilemap tilemap;
    public TileBase floorTile, wallTile, appleTile, snakeHeadTile, snakeBodyTile;

    public int xSize, ySize;

    private void Start()
    {
        print(1);
    }

    public void GenerateSnakeMap()
    {
        // 전체 바닥 타일 배치
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, floorTile);
            }
        }

        // 가장자리 벽 배치
        for (int i = 0; i < xSize; i++)
        {
            // 상하 벽
            tilemap.SetTile(new Vector3Int(i, 0, 0), wallTile);
            tilemap.SetTile(new Vector3Int(i, ySize - 1, 0), wallTile);

        }

        for (int i = 0; i < ySize; i++)
        {
            // 좌우 벽
            tilemap.SetTile(new Vector3Int(0, i, 0), wallTile);
            tilemap.SetTile(new Vector3Int(xSize - 1, i, 0), wallTile);
        }

       
    }

}
