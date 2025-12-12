using UnityEngine;
using UnityEngine.Tilemaps;

public class SnakeMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase floorTile, wallTile, appleTile;
    public int mapSize = 20;
    public int appleCount = 3;

    void Start()
    {
        GenerateSnakeMap();
    }

    void GenerateSnakeMap()
    {
        // 전체 바닥 타일 배치
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, floorTile);
            }
        }

        // 가장자리 벽 배치
        for (int i = 0; i < mapSize; i++)
        {
            // 상하 벽
            tilemap.SetTile(new Vector3Int(i, 0, 0), wallTile);
            tilemap.SetTile(new Vector3Int(i, mapSize - 1, 0), wallTile);
            // 좌우 벽
            tilemap.SetTile(new Vector3Int(0, i, 0), wallTile);
            tilemap.SetTile(new Vector3Int(mapSize - 1, i, 0), wallTile);
        }

        // 사과 랜덤 배치 (벽/사과 피해서)
        Vector3Int applePos;
        do
        {
            int x = Random.Range(1, mapSize - 1);
            int y = Random.Range(1, mapSize - 1);
            applePos = new Vector3Int(x, y, 0);
        } while (tilemap.GetTile(applePos) == wallTile || tilemap.GetTile(applePos) == appleTile);
        tilemap.SetTile(applePos, appleTile);
    }
}
