using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public TileBase obstacleTile; // 障碍物瓦片
    public TileBase borderTile;   // 边框瓦片

    // 示例输入数据
    public int mapWidth = 30;
    public int mapHeight = 30;

    // 障碍物数据，每个障碍物包含四个数字：x, y, 宽度, 高度
    public int[,] obstacles = {
        {0, 0, 30, 1},//障碍物: x, y, 宽度, 高度
        {0, 0, 1, 30},
        {0, 29, 30, 1},
        {29,0,1,30},
        {4,6,2,9},
        {12,9,7,2},
        {22,4,4,8},
        {11,14,2,2},
        {8,17,1,10},
        {16,15,3,9},
        {19,22,7,2}
    };

    void Start()
    {
        GenerateTilemap();
    }

    void GenerateTilemap()
    {
        // 创建一个新的 Grid 对象
        GameObject gridObject = new GameObject("TilemapGrid");
        Grid grid = gridObject.AddComponent<Grid>();

        // 创建 Tilemap 对象并将其添加到 Grid 中
        GameObject tilemapObject = new GameObject("Tilemap");
        Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
        tilemapObject.transform.parent = gridObject.transform;

        // 生成地图边框
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // 设置地图四周的边框
                if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), borderTile); // 设置边框瓦片
                }
            }
        }

        // 根据障碍物数据，在地图上添加障碍物
        for (int i = 0; i < obstacles.GetLength(0); i++) // 遍历障碍物数组的行
        {
            int startX = obstacles[i, 0]; // 障碍物的 x 坐标
            int startY = obstacles[i, 1]; // 障碍物的 y 坐标
            int width = obstacles[i, 2];  // 障碍物的宽度
            int height = obstacles[i, 3]; // 障碍物的高度

            // 将障碍物区域的瓦片设置为障碍物
            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), obstacleTile); // 设置障碍物瓦片
                }
            }
        }
    }
}
